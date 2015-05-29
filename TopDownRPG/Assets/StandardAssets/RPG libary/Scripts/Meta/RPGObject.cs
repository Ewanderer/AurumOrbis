using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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


/**
 * @author Jordan Eichner
 * \brief Diese Basisklasse vereinheitlicht die wichtigsten Eigenschaften aller Objekte, die Teil der vom RPG-System verwalteten Welt sind.
 */
public abstract class RPGObject:MonoBehaviour,IRPGSource
{

	//Standardwerte, jeweils die konstanten Grundwerte originalValue und ihr Pandon die currentValues, die mit Updatestatics neu bestimmt werden.
	
	protected float bWeight;/**< Standardgewicht in kg in einer Umgebung mit Standard g;*/
	protected float cWeight;/**< Durch Effekte verändertes Gewicht*/
	protected SizeCategory bSizeCategory; /**< Größenordnung für Modifikationen */
	protected SizeCategory cSizeCategory;	/**<Durch Effekte veränderte Größe*/	

	/**Geschützte Liste für Effekte ist über die entsprechenden Funktionen(AddEfect,RemoveEffect) von außen aufzurufen*/
	protected List<TEffect> cEffects = new List<TEffect> ();
	/**Liste von Informationen.*/
	protected List<Content> cInformation = new List<Content> ();

	public virtual float this [string ValueName] {
		get {
			string v = ValueName.ToLower ();
			if (v == "weight")
				return cWeight;
			if (v == "size")
				return (int)cSizeCategory;
			return 0;
			if (_Skills.Exists (delegate(Skill obj) {
				return obj.SkillName == ValueName;
			})) {
				Skill ss = _Skills.Find (delegate(Skill obj) {
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
		get{ return cEffects;}
	}
	/**Öffentliche Eigenschaft zum Auslesen des aktuellen Gewichts*/
	public virtual float Weight {
		get{ return cWeight;}
	}

	/**Öffentliche Eigenschaft zum Auslesen der Informationen, auch wen er nicht benutzt werden sollte, sondern man sollte eher @see GetInformation*/
	public virtual List<Content> Information {
		get{ return cInformation;}
	}



	/**
	 * \brief Diese Klasse verwaltet die Modifikationen für den genannten Wert, die sich aus den passiven Teilen aller Effekte ergeben
	 */
	public class AttributModificationHelper
	{
		public string AttributeName;/**<Name des Wertes, der hier berechnet wird.*/
		
		/**
		 * Diese innere Klasse dient zur Verwaltung aller passiven Effekten aus der Kategorie "add".
		 */
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
	
	public virtual bool AddEffect (TEffect Effect)
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
			UpdateStatistics ();//Aktualisiere die Statisitk
			return true;//Effekt erfolgreich hinzugefügt!
		}
		return false;//Effekt konnte nicht hinzugefügt werden!
	}


	//Diese Abgespeckte variante von Checkvale wird für das berechnen der Modifikationen für currentValues benutzt
	protected float GetCurrentValueModification (string Name)
	{
		return AttributeHelper.Find (delegate(AttributModificationHelper obj) {
			return obj.AttributeName == Name;
		}).OverallModification;
	}

	//Diese Funktion dient dazu die Funktionalität bei Add Effect zu gewährleisten, muss aber selbst aufgerufen werden, um Performance bei großen Effekt-Manipulierungen zu steigern.
	protected virtual void UpdateStatistics ()
	{
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
	public class Skill
	{
		TCreature Owner;
		public string SkillName;
		public string ShortDescription;
		public string LongDescription;
		int value;
		public string Attribute1;
		public string Attribute2;
		public string Attribute3;
		string[] BaseValues;
		public string[] Focus;
		public string[][] FocusAttributes;
		
		public int Value {
			get {
				int result = value;
				foreach (string bvs in BaseValues) {
					result += Mathf.RoundToInt (Owner._Skills.Find (delegate(Skill obj) {
						return bvs.Split (' ') [0] == obj.SkillName;
					}).Value * System.Convert.ToSingle (bvs.Split (' ') [1]));
				}
				return result;
			}
		}
		
	}
	
	
	protected List<Skill> _Skills = new List<Skill> ();
	
	public List<Skill> Skills {
		get{ return _Skills;}
	}

	//Abfrage über die Verfügbarkeit eines Skills
	public virtual void CheckSkill (string NameOfSkill, out int BaseValue, out int EndValue, out AttributModificationHelper.Modification[] UsedModification, out AttributModificationHelper.Counter[] UsedCounter)
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
		if ((ss = _Skills.Find (delegate(Skill obj) {
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
	public virtual string[] GetInformation (RPGObject Sender, bool IsDeepSearchesing=false)
	{
		return null;
	}

	//Update-Funktion für Effekte um ihre Restdauer im Auge zu behalten und um periodische Active-Strings auszulösen
	public void UpdateEffects (float timestep)
	{

	}

	//Funktionsrümpfe


//Diese Funktion dient zum Zugriff auf den HP-Wert oder so, gibt die Menge des angerichten schaden zurück. Heilungen. bzw Absorbtionen müssen an die RecieveHealing Funktion weitergegeben werden.	
	public abstract float RecieveDamage (float Value, string Typ, IRPGSource Source);

	//Dient zum Verrechnen von Heilung mit beispielsweise Heilmodifikationen
	protected abstract float RecieveHealing (float Value, IRPGSource Source);

	public virtual void SendMessage (string Message, IRPGSource Source)
	{

	}



}
