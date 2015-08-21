using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
/**
 *@author Jordan Eichner
 * 
 *\brief Diese Struktur dient einfach nur zur referenzierung der Größenkategorien
 * 
 * 
 */
public enum SizeCategory
{
	Fine,
	Diminutive,
	Tiny,
	Small,
	Medium,
	Large,
	Huge,
	Gargantum,
	Colossal
}
[System.Serializable]
public class CompactRPGObject {
	public string name="";
	public float bWeight=0f;/**< Standardgewicht in kg in einer Umgebung mit Standard g;*/
	public SizeCategory bSizeCategory=SizeCategory.Medium; /**< Größenordnung für Modifikationen */
	public TEffect[] cEffects;//Beim Deserialisieren benutzen wir diesen Wert
	public string[] bEffects;//Templates generieren ihre Effekten aus dieser Liste
	public Content[] Information;
	public ulong c;//Counter von dem Effekten
}

/**
 * @author Jordan Eichner
 * \brief Diese Basisklasse vereinheitlicht die wichtigsten Eigenschaften aller Objekte, die Teil der vom RPG-System verwalteten Welt sind.
 */
public abstract class RPGObject:IDComponent,IRPGSource
{

	public void createCompactRPGObject(CompactRPGObject o){
		o.bWeight = bWeight;
		o.bSizeCategory = cSizeCategory;
		o.cEffects =cEffects.ToArray();
		o.Information =cInformation.ToArray();
		o.c = IDCounter;
		o.name = _name;
	}

	//Mode bestimmt ob als Teil eines Templates oder aus einem File gesetzt werden soll. true ist hierbei ID, false=template
	public void setupObjectByCompact(CompactRPGObject o,bool mode){
		this._name = o.name;
		bWeight = o.bWeight;
		bSizeCategory = o.bSizeCategory;
		if (mode) {
			cEffects=new List<TEffect>();
			AttributeHelper=new List<AttributModificationHelper>();
			if(o.cEffects!=null)
			foreach(TEffect e in o.cEffects)
				OnNewEffect(e);
			IDCounter = o.c;
		}
		else
			if(o.bEffects!=null)
			foreach (string effSting in o.bEffects)
				OnNewEffect( RPGCore.instance.spawnEffect (effSting));
		if(o.Information!=null)
			cInformation =new List<Content>(o.Information);
		updateStatistics ();
	}


	[SerializeField]
	protected string _name;
	public string name{
		get{return _name;}
	}

	//Standardwerte, jeweils die konstanten Grundwerte originalValue und ihr Pandon die currentValues, die mit Updatestatics neu bestimmt werden.
	[SerializeField]
	protected float bWeight;/**< Standardgewicht in kg in einer Umgebung mit Standard g;*/
	[SyncVar]
	protected float cWeight;/**< Durch Effekte verändertes Gewicht*/
	
	[SerializeField]
	protected SizeCategory bSizeCategory; /**< Größenordnung für Modifikationen */
	[SyncVar]

	protected SizeCategory cSizeCategory;	/**<Durch Effekte veränderte Größe*/	

	/**Geschützte Liste für Effekte ist über die entsprechenden Funktionen(AddEfect,RemoveEffect) von außen aufzurufen*/
	[SerializeField]
	protected List<TEffect> cEffects = new List<TEffect> ();
	/**Liste von Informationen.*/
	[SerializeField]
	protected List<Content> cInformation = new List<Content> ();

	public virtual float this [string ValueName] {
		get {
			string v = ValueName.ToLower ();
			if (v == "weight")
				return cWeight;
			if (v == "size")
				return (int)cSizeCategory;
			return 0;
			if (_skills.Exists (delegate(Skill obj) {
				return obj.SkillName == ValueName;
			})) {
				Skill ss = _skills.Find (delegate(Skill obj) {
					return obj.SkillName == ValueName;
				});
				if (ss.Value > 0)
					return (int)(ss.Value + GetCurrentValueModification (ValueName));
				else
					return 0;
			}
		}
	}

	/**Öffentliche Eigenschaft zum Auslesen der aktuelle Größenordnung*/
	public virtual SizeCategory Size {
		get{ return cSizeCategory;}
	}
	/**Öffentliche Eigenschaft zum Auslesen aller Effekte, die auf das Objekt einwirken*/
	public virtual List<TEffect> Effects {
		get{ return new List<TEffect>(cEffects);}
	}
	/**Öffentliche Eigenschaft zum Auslesen des aktuellen Gewichts*/
	public virtual float Weight {
		get{ return cWeight;}
	}

	/**Öffentliche Eigenschaft zum Auslesen der Informationen, auch wen er nicht benutzt werden sollte, sondern man sollte eher @see GetInformation*/
	public virtual List<Content> Information {
		get{ return new List<Content>(cInformation);}
	}



	/**
	 * \brief Diese Klasse verwaltet die Modifikationen für den genannten Wert, die sich aus den passiven Teilen aller Effekte ergeben
	 */
	[SerializeField]
	public class AttributModificationHelper
	{
		public string AttributeName;/**<Name des Wertes, der hier berechnet wird.*/
		
		/**
		 * Diese innere Klasse dient zur Verwaltung aller passiven Effekten aus der Kategorie "add".
		 */
		[System.Serializable]
		public class Modification
		{

			/**
			 * \brief diese Funktion überprüft durch eine Suche in der Liste WorkingPassives ob der Effekt durchsucht wird 
			 * 
			 * \param Name Naja man muss den Attributsnamen rübergeben, damit man sicher überprüfen kann
			 * \return Der Name ist Programm...
			 */
			//?
			public bool IsSupressed (string Name)
			{
				
				foreach (string s in SourceEffect.WorkingPassiveEffectStrings) {
					if (s.Split (' ') [0].ToLower () == "add" && s.Split (' ') [1] == Name && s.Split (' ') [2] == SourceType && s.Split (' ') [3] == Order.ToString () && ((s.Contains ("%%") && Mode) || (!s.Contains ("%%") && !Mode)))
						return false;
				}
				return true;
			}

			public string SourceType;/**Angabe über den Sourcetyp, zur Kategorisierung<*/
			public float Value;/**<Gewährter Wert */
			public int Order;/**<*/
			public bool Mode;//false=additive,true=mulitplicativ
			public TEffect SourceEffect;
			public Modification (string sourcetype, float value, int order, bool mode, TEffect sourceeffect)
			{
				SourceType = sourcetype;
				Value = value;
				Order = order;
				Mode = mode;
				SourceEffect = sourceeffect;
			}
		}
		[System.Serializable]
		public class Inhibitor
		{
			public bool IsSupressed (string Name)
			{
				
				foreach (string s in SourceEffect.WorkingPassiveEffectStrings) {
					if (s.Split (' ') [0].ToLower () == "block" && s.Split (' ') [1] == Name && s.Split (' ') [2] == SourceTyp && s.Split (' ') [3] == Order.ToString ())
						return false;
				}
				return true;
			}
			
			public int Order;
			public string SourceTyp;
			public TEffect SourceEffect;
			
			public Inhibitor (string sourcetyp, int order, TEffect sourceffect)
			{
				SourceTyp = sourcetyp;
				this.Order = order;
				SourceEffect = sourceffect;
			}
			
		}
		[System.Serializable]
		public class Counter
		{
			public bool IsSupressed (string Name)
			{
				foreach (string s in SourceEffect.WorkingPassiveEffectStrings) {
					if (s.Split (' ') [0].ToLower () == "counter" && s.Split (' ') [1] == Name && s.Split (' ') [2] == SourceTyp && s.Split (' ') [3] == Order.ToString ())
						return false;
				}
				return true;
			}

			public int Order;
			public string SourceTyp;
			public TEffect SourceEffect;

			public Counter (int _Order, string _SourceTyp, TEffect _SourceEffect)
			{
				Order = _Order;
				SourceTyp = _SourceTyp;
				SourceEffect = _SourceEffect;
			}

		}

		public List<List<Modification>> AllModifications = new List<List<Modification>> ();//Diese Tabelle sortiert alle Effekte gleicher Herkunft in die einzelnen Spalten sortiert nach Order
		public List<Inhibitor> Inhibitors = new List<Inhibitor> ();//Selbes gilt für die Blocks
		public List<Counter> Counters = new List<Counter> ();

		//Diese dynamisch erstellte Liste wählt aus allen Spalten(Sourcetyp) den ersten Eintrag(höchste Order) aus, sofern dieser nicht durch einen Inhibitor mit höherer Order blockiert wird oder selbst unterdrückt wird.
		public List<Modification> UsedModifications {
			get {
				List<Modification> result = new List<Modification> ();
				for (int i=0; i<AllModifications.Count; i++) {
					if (!AllModifications [i] [0].IsSupressed (AttributeName) && (Inhibitors [0].IsSupressed (AttributeName) || Inhibitors [0].Order < AllModifications [i] [0].Order))
						result.Add (AllModifications [i] [0]);
				}
				return result;
			}
		}


		//Dieses Feld berechnet die GesamtModifikation
		public float OverallModification {
			get {
				List<Modification> effects = new List<Modification> ();
				for (int i=0; i<AllModifications.Count; i++) {
					if (!AllModifications [i] [0].IsSupressed (AttributeName) && (Inhibitors [0].IsSupressed (AttributeName) || Inhibitors [0].Order < AllModifications [i] [0].Order))
						effects.Add (AllModifications [i] [0]);
				}

				float Result = 0;
				foreach (Modification m in effects.FindAll(delegate(Modification obj) {
					return !obj.Mode;
				}))
					Result += m.Value;
				foreach (Modification m in effects.FindAll(delegate(Modification obj) {
					return obj.Mode;
				}))
					Result *= m.Value;
				return Result;
			}
		}

		public List<Counter> UsedCounter {
			get {
				List<Counter> result = new List<Counter> ();
				foreach (Counter c in Counters)
					if (!c.IsSupressed (AttributeName))
						result.Add (c);
				return result;
			}
		}
		
		public AttributModificationHelper (string Name)
		{
			AttributeName = Name;
		}
	}
	bool dirtAH;
	protected List<AttributModificationHelper> AttributeHelper = new List<AttributModificationHelper> ();

	//Diese Funktion baut einen neuen Effekt in die Helper ein
	protected void OnNewEffect (TEffect effect)
	{
		foreach (string eff in effect.PassiveEffectStrings) {
			string[] splitted = eff.Split (' ');
			//Überprüfen ob es ein Modifikator oder ein Inhibitor ist.
			if (splitted [0] == "add") {
				//Platz für den Wert machen, sofern nicht vorhanden
				if (!AttributeHelper.Exists (delegate(AttributModificationHelper obj) {
					return obj.AttributeName == splitted [1];
				}))
					AttributeHelper.Add (new AttributModificationHelper (splitted [1]));
				//Entsprechenden AttributsHelfer auswählen
				AttributModificationHelper AMH = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
					return obj.AttributeName == splitted [1];
				});
				//Evt. Platz für die neue Quelle machen und Effekt direkt einfügen
				if (!AMH.AllModifications.Exists (delegate(List<AttributModificationHelper.Modification> obj) {
					return obj [0].SourceType == splitted [2];
				})) {
					AMH.AllModifications.Insert (0, new List<AttributModificationHelper.Modification> ());
					AMH.AllModifications [0].Add (new AttributModificationHelper.Modification (splitted [2], System.Convert.ToSingle (splitted [4].TrimEnd ('%').TrimEnd ('%')), System.Convert.ToInt16 (splitted [3]), splitted [4].Contains ("%%"), effect));
				} else
					AMH.AllModifications.Find (delegate(List<AttributModificationHelper.Modification> obj) {
						return obj [0].SourceType == splitted [2];
					}).Add (new AttributModificationHelper.Modification (splitted [2], System.Convert.ToSingle (splitted [4].TrimEnd ('%').TrimEnd ('%')), System.Convert.ToInt16 (splitted [3]), splitted [4].Contains ("%%"), effect));
				
			} else
			if (splitted [0] == "block") {
				//Platz für den Wert machen, sofern nicht vorhanden
				if (!AttributeHelper.Exists (delegate(AttributModificationHelper obj) {
					return obj.AttributeName == splitted [1];
				}))
					AttributeHelper.Add (new AttributModificationHelper (splitted [1]));
				//Entsprechenden AttributsHelfer auswählen
				AttributModificationHelper AMH = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
					return obj.AttributeName == splitted [1];
				});
				//Evt. Platz für die neue Quelle machen und Effekt direkt einfügen
				AMH.Inhibitors.Add (new AttributModificationHelper.Inhibitor (splitted [2], System.Convert.ToInt16 (splitted [3]), effect));
			} else {
				if (splitted [0] == "counter") {
					if (!AttributeHelper.Exists (delegate(AttributModificationHelper obj) {
						return obj.AttributeName == splitted [1];
					}))
						AttributeHelper.Add (new AttributModificationHelper (splitted [1]));
					AttributModificationHelper AMH = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
						return obj.AttributeName == splitted [1];
					});
					//Evt. Platz für die neue Quelle machen und Effekt direkt einfügen
					AMH.Counters.Add (new AttributModificationHelper.Counter (System.Convert.ToInt16 (splitted [3]), splitted [2], effect));
				}
			}
		}
	}
	
	//Diese Funktion entfernt den Effekt aus allen Helpern
	protected void OnRemoveEffect (TEffect effect)
	{
		foreach (string s in effect.PassiveEffectStrings) {
			string[] splitted = s.Split (' ');
			//Überprüfe ob effect ein Modifiktor oder Inhibitor war
			if (splitted [0] == "add" || splitted [0] == "block" || splitted [0] == "counter") {
				//Wähle den entsprechenden AttributsHelfer aus
				AttributModificationHelper AMH = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
					return obj.AttributeName == splitted [1];
				});
				//Finde die Liste der Modifikation mit selber SourceType
				List<AttributModificationHelper.Modification> ml = AMH.AllModifications.Find (delegate(List<AttributModificationHelper.Modification> obj) {
					return obj [0].SourceType == splitted [2];
				});
				//Lösche Einträge in Inhibitoren
				AMH.Inhibitors.RemoveAll (delegate(AttributModificationHelper.Inhibitor obj) {
					return obj.SourceEffect == effect;
				});
				//Lösche Einträge in der ModifikationsListe
				ml.RemoveAll (delegate(AttributModificationHelper.Modification obj) {
					return obj.SourceEffect == effect;
				});
				//Lösche Einträge aus den COunter
				AMH.Counters.RemoveAll (delegate(AttributModificationHelper.Counter obj) {
					return obj.SourceEffect == effect;
				});

				//Eine leere Source-Liste wird entfernt
				if (ml.Count == 0)
					AMH.AllModifications.Remove (ml);
				//Ein leerer Attributshelfer wird entfernt...
				if (AMH.AllModifications.Count == 0 && AMH.Inhibitors.Count == 0 && AMH.Counters.Count == 0)
					AttributeHelper.Remove (AMH);
			}
		}
	}

	protected void EnforceEffectSupression ()
	{
		//Vorbereitung der Supression
		foreach (TEffect e in cEffects) { 
			//Hebe alle Supressions auf
			e.WorkingPassiveEffectStrings = new List<string> (e.PassiveEffectStrings);
			e.IsSupressed = false;
		}
		//Sortiere alle Effekte, nach Odnung von Supression-Effekten
		cEffects.Sort (delegate(TEffect x, TEffect y) {
			//Supress Category>Supress Sourcetyp>kein supress
			string[] supressionStringX = x.WorkingPassiveEffectStrings.Find (delegate(string obj) {
				return obj.Split (' ') [0] == "supress";
			}).Split (' ');
			string[] supressionStringY = y.WorkingPassiveEffectStrings.Find (delegate(string obj) {
				return obj.Split (' ') [0] == "supress";
			}).Split (' ');
			if (supressionStringX [0] == "" && supressionStringY [0] == "")
				return 0;
			if (supressionStringX [0] != "" && supressionStringY [0] == "")
				return 1;
			if (supressionStringX [0] == "" && supressionStringY [0] != "")
				return -1;
			//Muss überprüfen ob das Sinn macht, Category über SourceTyp zu stellen :D
			/*
			if(supressionStringX[1]=="category"&&supressionStringY[0]=="sourcetyp")
				return 1;
			if(supressionStringX[1]=="sourcetyp"&&supressionStringY[0]=="category")
				return -1;*/
			return System.Convert.ToInt16 (supressionStringX [3]) - System.Convert.ToInt16 (supressionStringY [3]);
		});
		//Arbeite nun nacheinander alle Effekte ab
		for (int i=0; i<cEffects.Count; i++) {
			//Ein Unterdrückter effekt kann keine anderen effekte unterdrücken
			if (!cEffects [i].IsSupressed) {
				//Untersuche ob es eine Supression gibt, die nicht selber unterdrückt wird.
				string[] suppression = cEffects [i].WorkingPassiveEffectStrings.Find (delegate(string obj) {
					return obj.Split (' ') [0] == "supress";
				}).Split (' ');
				if (suppression [0] != "") {
					//Wenn sie exestiert wend sie in allen folgenden Effekten an
					for (int c=i+1; c<cEffects.Count; c++) {
						if (suppression [1] == "category" && (cEffects [c].GeneralCategory == suppression [2] || cEffects [c].Tags.Contains (suppression [2])) && cEffects [c].GeneralOrder < System.Convert.ToInt16 (suppression [3]))
							cEffects [c].IsSupressed = true;
						else
						if (suppression [1] == "sourcetyp") {
							int porder = System.Convert.ToInt16 (suppression [3]);
							cEffects [c].WorkingPassiveEffectStrings.RemoveAll (delegate(string obj) {
								string[] effsting = obj.Split (' ');
								if (effsting [0] == "add" || effsting [0] == "block" || effsting [0] == "counter")
								if (effsting [2] == suppression [2] && System.Convert.ToInt16 (effsting [2]) < porder)
									return true;
								if (effsting [0] == "supress" || effsting [1] == "protect")
								if (effsting [4] == suppression [2] && System.Convert.ToInt16 (effsting [3]) < porder)
									return true;
								return false;
							});
						}
					}
				}
			}
		}
	}




	[SyncVar]
	[SerializeField]		 
	ulong IDCounter=0;		                           

	public bool addEffect(TEffect effect,IRPGSource source){
		MyNetworkManager.DistributeIDComponentUpdate (this, new IDComponentUpdateMsg (1, FileHelper.serializeObject<AddEffectMsg> (new AddEffectMsg (effect, source.getID ()))));
		return _addEffect (effect, source);
	}

	protected virtual bool _addEffect (TEffect Effect,IRPGSource source)
	{
		//Überprüfe zunächst ob ein Schutz gegen den Effekt exestiert
		if (!cEffects.Exists (delegate(TEffect obj) {
			foreach (string effs in obj.WorkingPassiveEffectStrings) {
				string[] splitted = effs.Split (' ');
				if (splitted [0] == "protectagainst" && (splitted [1] == Effect.GeneralCategory || Effect.Tags.Contains (splitted [1])) && System.Convert.ToInt16 (splitted [2]) > Effect.GeneralOrder)
					return true;

			}
			return false;
		})) {
			OnNewEffect (Effect);//Füge den neuen Effekt in das Helper System ein
			cEffects.Add (Effect);//Füge ihn in die Liste der Effekte ein
			Effect.HookUp(source,this,IDCounter++);
		//	UpdateStatistics ();//Aktualisiere die Statisitk(wird erst nach anknüpfen aller Effekte von der Gegenseite aufgerufen!)
			return true;//Effekt erfolgreich hinzugefügt!
		}
		return false;//Effekt konnte nicht hinzugefügt werden!
	}

	public bool removeEffect(TEffect effect,bool enforceRemove=false){
		MyNetworkManager.DistributeIDComponentUpdate (this, new IDComponentUpdateMsg (2,FileHelper.serializeObject<RemoveEffectMsg>(new RemoveEffectMsg(effect.getID(),enforceRemove))));
		return _removeEffect (effect, enforceRemove);
	}

	protected virtual bool _removeEffect(TEffect effect,bool enforceRemove=false){

		if (enforceRemove && effect.oDuration != -1) {
			OnRemoveEffect (effect);
			cEffects.Remove (effect);
			return true;
		}
		return false;
	}


	//Diese Abgespeckte variante von Checkvale wird für das berechnen der Modifikationen für currentValues benutzt
	protected float GetCurrentValueModification (string Name)
	{
		AttributModificationHelper amh;
		if ((amh = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
			return obj.AttributeName == Name;
		})) != default(AttributModificationHelper))
			return amh.OverallModification;
		else
			return 0;
	}



	//Diese Funktion dient dazu die Funktionalität bei Add Effect zu gewährleisten, muss aber selbst aufgerufen werden, um Performance bei großen Effekt-Manipulierungen zu steigern.

	public virtual void updateStatistics ()
	{
		dirtAH = true;
		EnforceEffectSupression ();
		//Sortiere die Listen der AttributsHelfer
		for (int i=0; i<AttributeHelper.Count; i++) {
			for (int ii=0; i<AttributeHelper[i].AllModifications.Count; ii++) {
				AttributeHelper [i].AllModifications [ii].Sort (delegate(AttributModificationHelper.Modification x, AttributModificationHelper.Modification y) {
					if (x.IsSupressed (AttributeHelper [i].AttributeName) && y.IsSupressed (AttributeHelper [i].AttributeName))
						return 0;
					if (!x.IsSupressed (AttributeHelper [i].AttributeName) && y.IsSupressed (AttributeHelper [i].AttributeName))
						return 1;
					if (x.IsSupressed (AttributeHelper [i].AttributeName) && !y.IsSupressed (AttributeHelper [i].AttributeName))
						return -1;
					if (x.Order > y.Order)
						return 1;
					if (x.Order < y.Order)
						return -1;
					return 0;
				});
				
				
			}
			AttributeHelper [i].Inhibitors.Sort (delegate(AttributModificationHelper.Inhibitor x, AttributModificationHelper.Inhibitor y) {
				if (x.IsSupressed (AttributeHelper [i].AttributeName) && y.IsSupressed (AttributeHelper [i].AttributeName))
					return 0;
				if (!x.IsSupressed (AttributeHelper [i].AttributeName) && y.IsSupressed (AttributeHelper [i].AttributeName))
					return 1;
				if (x.IsSupressed (AttributeHelper [i].AttributeName) && !y.IsSupressed (AttributeHelper [i].AttributeName))
					return -1;
				if (x.Order > y.Order)
					return 1;
				if (x.Order < y.Order)
					return -1;
				return 0;
			});
		}

		cWeight = bWeight + GetCurrentValueModification ("weight");
		cSizeCategory = bSizeCategory + (int)GetCurrentValueModification ("sizecategory");

		//Setzten der Statuseffekte

	}
	//Feritkgieten
	[System.Serializable]
	public class Skill
	{
		[SerializeField]
		RPGObject Owner;
		public string SkillName;
		public string ShortDescription;
		public string LongDescription;
		[SerializeField]
		int value;
		public string Attribute1;
		public string Attribute2;
		public string Attribute3;
		[SerializeField]
		string[] BaseValues;
		public string[] Focus;
		public string[] FocusAttributes;
		
		public int Value {
			get {
				int result = value;
				foreach (string bvs in BaseValues) {
					result += Mathf.RoundToInt (Owner._skills.Find (delegate(Skill obj) {
						return bvs.Split (' ') [0] == obj.SkillName;
					}).Value * System.Convert.ToSingle (bvs.Split (' ') [1]));
				}
				return result;
			}
		}
		
	}

	[SerializeField]
	protected List<Skill> _skills = new List<Skill> ();
	public List<Skill> Skills {
		get{ 
			return new List<Skill>(_skills);
		}
	}

	//Skill-Sachen

	//Lädt aus der Datenbank den Basisskill
	public void addSkill(string SkillName){
		MyNetworkManager.DistributeIDComponentUpdate (this, new IDComponentUpdateMsg (4, FileHelper.serializeObject<string> ("+" + SkillName)));
		_addSkill (SkillName);
	}
	
	//Aktiviert aus der Datenbank den Basisskill
	public void addSkillFocus(string SkillName,string FokusName,string Attributes=""){
		MyNetworkManager.DistributeIDComponentUpdate (this, new IDComponentUpdateMsg (4, FileHelper.serializeObject<string> (SkillName + "+" + FokusName + "_" + Attributes)));
		_addSkillFocus (SkillName, FokusName, Attributes);
	}

	public void setSkillAttributes(string SkillName,string a1="",string a2="",string a3=""){
		MyNetworkManager.DistributeIDComponentUpdate (this, new IDComponentUpdateMsg (4, FileHelper.serializeObject<string> (SkillName+ "<" + a1+","+a2+","+a3)));
		_setSkillAttributes (SkillName, a1, a2, a3);
	}

	protected void _addSkill(string SkillName){

	}

	protected void _addSkillFocus(string SkillName,string FokusName,string Attributes=""){
	
	}

	protected void _setSkillAttributes(string SkillName,string a1="",string a2="",string a3=""){
	
	}



	//Abfrage über die Verfügbarkeit eines Skills

	public virtual void checkSkill (string NameOfSkill, out int BaseValue, out int EndValue, out AttributModificationHelper.Modification[] UsedModification, out AttributModificationHelper.Counter[] UsedCounter)
	{
		BaseValue = 0;
		EndValue = 0;
		string[] Parts = NameOfSkill.Split ('-');
		NameOfSkill = Parts [0];

		float mod = GetCurrentValueModification (NameOfSkill);
		UsedModification = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
			return obj.AttributeName == NameOfSkill;
		}).UsedModifications.ToArray ();
		UsedCounter = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
			return obj.AttributeName == NameOfSkill;
		}).Counters.ToArray ();
		if (mod < 0)
			EndValue -= (int)mod;
		Skill ss;
		if ((ss = Skills.Find (delegate(Skill obj) {
			return obj.SkillName == NameOfSkill;
		}))!=default(Skill)) {
			BaseValue = ss.Value;
			if (ss.Value > 0) {
				EndValue = (int)(ss.Value + mod);
				if (Parts.Length > 1) {
					for (int i=0; i<ss.Focus.Length; i++)
						if (ss.Focus [i] == Parts [1]) {
							EndValue +=(int) GetCurrentValueModification (Parts [0] + '-' + Parts [1]);
							return;
						}
					BaseValue=0;
					UsedModification=null;
					UsedCounter=null;
					EndValue = 0;
				}
			}
		}
	}

	//Informationen
	//Diese Funktion benutzt das Objekt um Informationen über das Target zu sammeln. DeepSearch offenbart alle Informationen die das StrongRequirements "NeedAnalyze" haben 
	public virtual string[] getInformation (RPGObject Sender, bool IsDeepSearchesing=false)
	{
		return null;
	}

	//Update-Funktion für Effekte um ihre Restdauer im Auge zu behalten und um periodische Active-Strings auszulösen
	public void UpdateEffects (float timestep)
	{
		for (int i=0; i<Effects.Count; i++) {
			if (Effects [i].oDuration != -1) {
				Effects[i].cDuration-=timestep;
				if(Effects[i].cDuration<=0){
					Effects.RemoveAt(i);
					i--;
					continue;
				}
			}
			foreach (EffectScriptObject eo in Effects[i].ScriptObjects) {
				eo.OnTimer ();
			}
		}
	}

	//Funktionsrümpfe


//Diese Funktion dient zum Zugriff auf den HP-Wert oder so, gibt die Menge des angerichten schaden zurück. Heilungen. bzw Absorbtionen müssen an die RecieveHealing Funktion weitergegeben werden.	
	public float recieveDamage(float _value,string _typ,IRPGSource _source){
		MyNetworkManager.DistributeIDComponentUpdate (this, new IDComponentUpdateMsg (3, FileHelper.serializeObject<string> (_value.ToString()+";"+_typ+";"+_source.getID())));
		return _recieveDamage (_value, _typ, _source);
	}

	protected abstract float _recieveDamage (float Value, string Typ, IRPGSource Source);
	//Dient zum Verrechnen von Heilung mit beispielsweise Heilmodifikationen
	protected abstract float recieveHealing (float Value, IRPGSource Source);

	public virtual void sendMessage (string Message, IRPGSource Source)
	{

	}

	public string getID(){
		return base._referenceID;
	}

	public override void initialize ()
	{
		base.initialize ();
	}

	public override void deserializeFromFile (string FileName)
	{
		CompactRPGObject cO = FileHelper.deserializeObject<CompactRPGObject> (FileHelper.ReadFromFile (FileName));
		setupObjectByCompact (cO, true);
	}
	public override void serializeToFile (string FileName)
	{
		CompactRPGObject cO = new CompactRPGObject ();
		this.createCompactRPGObject(cO);
		FileHelper.WriteToFile (FileName, FileHelper.serializeObject<CompactRPGObject>(cO));
	}


	public override void OnNetworkUpdate (NetworkMessage msg)
	{
		IDComponentUpdateMsg M = msg.ReadMessage<IDComponentUpdateMsg> ();
		switch (M.updateType) {
		case 0:
			setupObjectByCompact(FileHelper.deserializeObject<CompactRPGObject>(M.data),true);
			break;
		case 1:
			AddEffectMsg aem=FileHelper.deserializeObject<AddEffectMsg>(M.data);
			if(MyNetworkManager.instance.mode)
				_addEffect(aem.effect,WorldGrid.getSource(aem.sourceID));
			else
				_addEffect(aem.effect,Watcher.getReferenceSource(aem.sourceID));
			break;
		case 2:
			RemoveEffectMsg rem=FileHelper.deserializeObject<RemoveEffectMsg>(M.data);
				_removeEffect(cEffects.Find(delegate(TEffect obj) {
					return obj.getID()==rem.effectID;
				}),rem.doenforce);
			break;
		case 3:
			string damageString=FileHelper.deserializeObject<string>(M.data);
			if(MyNetworkManager.instance.mode)
				_recieveDamage(System.Convert.ToSingle(damageString.Split(';')[0]),damageString.Split(';')[1],WorldGrid.getSource(damageString.Split(';')[2]));
			else
				_recieveDamage(System.Convert.ToSingle(damageString.Split(';')[0]),damageString.Split(';')[1],Watcher.getReferenceSource(damageString.Split(';')[2]));
			break;
		case 4:
			string skillString=FileHelper.deserializeObject<string>(M.data);
			if (skillString.Contains ("+")) {
				if(skillString[0]=='+'){
					_addSkill(skillString.TrimStart('+'));
				}else{
					string a="";
					if(skillString.Contains("_"))
						a=skillString.Split('_')[1];
					_addSkillFocus(skillString.Split('+')[0],skillString.Split('_')[0],a);
				}
			}else
			if (skillString.Contains ("<")) {
				_setSkillAttributes(skillString.Split('<')[0],skillString.Split('<')[1].Split(',')[0],skillString.Split('<')[1].Split(',')[1],skillString.Split('<')[1].Split(',')[2]);
			}
			break;
		}
	}

	public override IDComponentUpdateMsg CreateInitialSetupMessage ()
	{
		IDComponentUpdateMsg result = new IDComponentUpdateMsg ();
		result.componentName = GetType ().ToString ();
		result.id = getID ();
		result.updateType = 0;
		CompactRPGObject obj = new CompactRPGObject ();
		createCompactRPGObject (obj);
		result.data = FileHelper.serializeObject<CompactRPGObject> (obj);
		return result;
	}
}
