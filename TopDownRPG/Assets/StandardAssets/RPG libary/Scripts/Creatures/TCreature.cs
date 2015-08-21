using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class CompactCreature:CompactRPGObject{
	public int bStrength;
	public int bCourage;
	public int bAgility;
	public int bPrestidigitation;
	public int bConstitution;
	public int bMetabolism;
	public int bIntelligence;
	public int bWisdom;
	public int bCharisma;
	public int bAppearance;
	
	public float cVitality;//Aktuelle Gesundheit wenn sie auf 0 fällt stirbt der Char, können nicht regeneriert werden solange nicht aller Schmerz wiederhergestellt wurde und auch benötigt man ärtzliche Behandlung.]
	public float cHitpoints;//Eine Art Schutz gegen Schwere Verwundungen. Wird durch Rast, Heilmittelchen oder einfache Heilzauber geschaffen. 
	public float cCondition;//Der Grad der Erschöpfung durch langanhaltende Belastung(Tragen Schwerer Rüstung, Schlafmangel). Stellt auch die Maximale Grenze für Stamina-Regeneration da.
	public float cStamina;//Wird durch Kurzeitige Körperliche Aktivität benötigt
	
	public TCreature.BodyPart bodyRoot;
	

	public void createFromTemplate(TCreature_Template template){
		bStrength = 10;
		bCourage = 10;
		bAgility = 10;
		bPrestidigitation = 10;
		bConstitution = 10;
		bMetabolism = 10;
		bIntelligence = 10;
		bWisdom = 10;
		bAppearance = 10;
		bCharisma = 10;
		bodyRoot = new TCreature.BodyPart (template.rootLimb);
		bSizeCategory = template.size;
		bEffects = new string[1];
		bEffects [0] = template.linkedEffect.Name;
	}
}
//Diese Klasse repräsentiert den Charakterbogen und dient dem Zusammenfassen der Auswirkung aller Statuseffekte und Ausrüstungseffekte.
[RequireComponent(typeof(Rigidbody))]
public class TCreature : RPGObject
{
	public void createCompactCreature (CompactCreature o)
	{
		base.createCompactRPGObject (o);
		o.bStrength = bStrength;
		o.bCourage = bCourage;
		o.bAgility = bAgility;
		o.bPrestidigitation = bPrestidigitation;
		o.bConstitution = bConstitution;
		o.bMetabolism = bMetabolism;
		o.bIntelligence = bIntelligence;
		o.bWisdom = bWisdom;
		o.bCharisma = bCharisma;
		o.bAppearance = bAppearance;
		o.cVitality = _cVitality;
		o.cHitpoints = _cHitpoints;
		o.cCondition = _cCondition;
		o.cStamina = _cStamina;
	}
	
	public void setupObjectByCompact (CompactCreature o,bool mode)
	{
		base.setupObjectByCompact (o, mode);
		bStrength = o.bStrength;
		bCourage = o.bCourage;
		bAgility = o.bAgility;
		bPrestidigitation = o.bPrestidigitation;
		bConstitution = o.bConstitution;
		bMetabolism = o.bMetabolism;
		bIntelligence = o.bIntelligence;
		bWisdom = o.bWisdom;
		bAppearance = o.bAppearance;
		bCharisma = o.bCharisma;
		updateStatistics ();
		if (mode) {
			_cVitality=o.cVitality;
			_cHitpoints=o.cHitpoints;
			_cCondition=o.cCondition;
			_cStamina=o.cStamina;
		}else{
			_cVitality=_mVitality;
			_cHitpoints=_mVitality;
			_cCondition=mCondition;
			_cStamina=mCondition;
		}
		_bodyRoot = bodyRoot;
	} 
	
	//Diese Hilfsklasse dient zur Serialisierung der Kreature

	public override void serializeToFile (string FileName)
	{
		CompactCreature cC = new CompactCreature ();
		createCompactCreature (cC);
		FileHelper.WriteToFile (FileName, FileHelper.serializeObject<CompactCreature> (cC));
	}

	public override void deserializeFromFile (string FileName)
	{
		CompactCreature cC = FileHelper.deserializeObject<CompactCreature>(FileHelper.ReadFromFile (FileName));
		setupObjectByCompact (cC,true);
	}

	public override List<TEffect> Effects {
		get {
			List<TEffect> result = new List<TEffect> ();
			result.AddRange (cEffects);
				result.AddRange (_bodyRoot.cEffects);
			return result.FindAll (delegate(TEffect obj) {
				return !obj.IsSupressed;
			});
		}
	}
	//Charakter-Statistiken

	//Primäre Attribute

	//Felder für die Basiswerte aller Primären Attribute, können nur durch "Training"/LvL-Up verändert werden
	[SerializeField]
	int bStrength;
	[SerializeField]
	int bCourage;
	[SerializeField]
	int bAgility;
	[SerializeField]
	int bPrestidigitation;
	[SerializeField]
	int bConstitution;
	[SerializeField]
	int bMetabolism;
	[SerializeField]
	int bIntelligence;
	[SerializeField]
	int bWisdom;
	[SerializeField]
	int bCharisma;
	[SerializeField]
	int bAppearance;
	//Felder für die durch Effekte modifizierten Attribute
	[SyncVar]
	[System.NonSerialized]
	int cStrength;
	[SyncVar]
	[System.NonSerialized]
	int cCourage;
	[SyncVar]
	[System.NonSerialized]
	int cAgility;
	[SyncVar]
	[System.NonSerialized]
	int cPrestidigitation;
	[SyncVar]
	[System.NonSerialized]
	int cConstitution;
	[SyncVar]
	[System.NonSerialized]
	int cMetabolism;
	[SyncVar]
	[System.NonSerialized]
	int cIntelligence;
	[SyncVar]
	[System.NonSerialized]
	int cWisdom;
	[SyncVar]
	[System.NonSerialized]
	int cCharisma;
	[SyncVar]
	[System.NonSerialized]
	int cAppearance;
	//Öffentliche Felder zum Auslesen der Attribute


	public int Strength {
		get{ return cStrength;}
	}
	public int Courage {
		get{ return cCourage;}
	}

	public int Agility {
		get{ return cAgility;}
	}

	public int Prestidigitation{
		get{return cPrestidigitation;}
	}

	public int Constitution {
		get{ return cConstitution;}
	}

	public int Metabolism {
		get{ return cMetabolism;}
	}

	public int Intelligence {
		get{ return cIntelligence;}
	}

	public int Wisdom {
		get{ return cWisdom;}
	}

	public int Charisma {
		get{ return cCharisma;}
	}

	public int Appearance {
		get{ return cAppearance;}
	}

	public override float this [string ValueName] {
		get {
			switch (ValueName.ToLower ()) {
			case "strength":
				return Strength;
			case "courage":
				return Courage;
			case "constitution":
				return Constitution;
			case "agility":
				return Agility;
			case "prestidigitation":
				return Prestidigitation;
			case "metabolism":
				return Metabolism;
			case "intelligence":
				return Intelligence;
			case "wisdom":
				return Wisdom;
			case "charisma":
				return Charisma;
			case "appearance":
				return Appearance;
			}

			return base [ValueName];
		}
	}

	//Sekundäre Attribute

	float _bVitality{//Maximale Gesunheit, Grundwert
		get{return Constitution*2;}
	}
	[SyncVar]
	[System.NonSerialized]
	float _mVitality;//Maximale Gesundheit, Endwert.
	[SyncVar]
	float _cVitality;//Aktuelle Gesundheit wenn sie auf 0 fällt stirbt der Char, können nicht regeneriert werden solange nicht aller Schmerz wiederhergestellt wurde und auch benötigt man ärtzliche Behandlung.
	[SyncVar]
	float _cHitpoints;//Eine Art Schutz gegen Schwere Verwundungen. Wird durch Rast, Heilmittelchen oder einfache Heilzauber geschaffen. 

	float _bCondition{//Die Gesamtasudauer der Figur,Grundwert
		get{return Constitution*2;}
	}
	[SyncVar]
	[System.NonSerialized]
	float _mCondition;//Die Gesamtausdauer der Figur, Endwert
	[SyncVar]
	[SerializeField]
	float _cCondition;//Der Grad der Erschöpfung durch langanhaltende Belastung(Tragen Schwerer Rüstung, Schlafmangel). Stellt auch die Maximale Grenze für Stamina-Regeneration da.
	[SyncVar]
	[SerializeField]
	float _cStamina;//Wird durch Kurzeitige Körperliche Aktivität benötigt
	[SerializeField]
	float _bMana;//Maximaler Manapool, wenn 0 können keine anderen Effekte die Mana steigern greifen.
	[SyncVar]
	float _mMana;
	[SyncVar]
	[SerializeField]
	float _cMana;
	float _bInitative{
		get{return (cWisdom+cIntelligence+cCourage)/3;}
	}


	//Öffentliche Felder für Sekundär-Attribute

	public float mVitality {
		get{ return _mVitality;}
	}

	public float cVitality {
		get{ return _cVitality;}
	}

	public float cHitpoints {
		get{ return _cHitpoints;}
	}

	public float mCondition {
		get{ return _mCondition;}
	}

	public float cCondition {
		get{ return _cCondition;}
	}

	public float cStamina {
		get{ return _cStamina;}
	}

	public float mMana {
		get{ return _mMana;}
	}

	public float cMana {
		get{ return _cMana;}
	}

	public float cInitative{
		get{return cInitative;}
	}


	/*
	public bool this[string ValueName,int i]{
		get{return GetBBaseValue(ValueName.ToLower())&&GetCurrentValueModification(ValueName)>0;}
	}*/

	//Ausrüstung und Inventar
	[System.Serializable]
	public struct BodyPart
	{
		public string name;
		
		
		public string equipmentName;//Art welche Art von Equipment hier angeelgt werden darf
	
		public int weaponSize;//Gibt auskunft ob und wie groß eine Waffe sein kann. 0=keine Waffe.
		public int pairNr;//Für zweihändige Waffen benötigt man zwei geeignete mit selben PairNr.-Wert Limbs.
		
		public float vital;//Vitale Punkte kosten jeweils einen festen Teil an der maximalen Gesundheit bei Verlust. Ist dieser wert 1, bedeutet der Verlust Tod
		public float regrowthRate;//Anzahl der Rasten bis dieses Limb regeniert wurde. -1 kann nicht regenerieren.


		//Equipmentpart
	//	public string slotType;//In den Equipmentslot kommen nur TEquipment objekte vom Typ der hier angegen ist :D
		public SizeCategory size;//Für Beschränkungen von Equipment
		string slottedItemID;
		[System.NonSerialized]
		TEquipment _slottedItem;
		public TEquipment slottedItem{
			get{
				if(_slottedItem&&_slottedItem.getID()==slottedItemID)
					return _slottedItem;
				if(NetworkServer.active)
					_slottedItem=WorldGrid.getIDObject<TEquipment>(slottedItemID);
				if(NetworkClient.active&&!NetworkServer.active)
					_slottedItem=Watcher.getReferenceObject<TEquipment>(slottedItemID);
				return _slottedItem;
			}
			set{
				slottedItemID=value.getID();
			}
		}

		//Subparts
		BodyPart[] subPart;

		//Rekusive Suche nach allen Effekten
		public List<TEffect> cEffects{
			get{
				List<TEffect> result=new List<TEffect>(_slottedItem.Effects);
				foreach(BodyPart p in subPart)
					result.AddRange(p.cEffects);
				return result;
			}
		}

		public BodyPart(TCreatureLimb origin){
			this.name=origin.name;
			this.pairNr=origin.pairNr;
			this.equipmentName=origin.equipmentName;
			this.regrowthRate=origin.regrowthRate;
			this.size=origin.size;
			this.slottedItemID="";
			this.slottedItem=null;
			this.weaponSize=origin.weaponSize;
			subPart=new BodyPart[origin.subLimbs.Length];
			for(int i=0;i<subPart.Length;i++)
				subPart[i]=new BodyPart(origin.subLimbs[i]);
		}
	}
	[SerializeField]
	BodyPart _bodyRoot;
	[SerializeField]
	List<TItem> _Inventory = new List<TItem> ();

	public BodyPart bodyRoot {
		get{ return _bodyRoot;}
	}

	public List<TItem> Inventory {
		get{ return _Inventory;}
	}	

	//Statuses

	public enum CreatureMovementStatus
	{
		Resting=0,
		Idle=1,
		Sneaking=2,
		Walking=3,
		Running=4,
		SelfLevitation=5
	}
	;
	[SerializeField]
	[SyncVar]
	CreatureMovementStatus CMS;
	[SerializeField]
	[SyncVar]
	bool IsAlive;//Well Undeads are also alive :D



	//Diese Funktion wird bei jeder Veränderung von Equipment oder Effektliste neu aufgerufen
	public override void updateStatistics ()
	{
		base.updateStatistics();
		//Berechne die current Values zur schnellerern Abgreifung, Primäre Attribute
		cStrength = bStrength + (int)GetCurrentValueModification ("strength");
		cCourage = bCourage + (int)GetCurrentValueModification ("courage");
		cAgility = bAgility + (int)GetCurrentValueModification ("agility");
		cPrestidigitation = bPrestidigitation + (int)GetCurrentValueModification ("prestidigitation");
		cConstitution = bConstitution + (int)GetCurrentValueModification ("constitution");
		cMetabolism = bMetabolism + (int)GetCurrentValueModification ("metabolism");
		cIntelligence = bIntelligence + (int)GetCurrentValueModification ("intelligence");
		cWisdom = bWisdom + (int)GetCurrentValueModification ("wisdom");
		cCharisma = bCharisma + (int)GetCurrentValueModification ("charisma");
		cAppearance = bAppearance + (int)GetCurrentValueModification ("appearance");

		//Berechene die sekundäre Attribute
		_bMana = GetCurrentValueModification ("manapool");
		if (_bMana > 0) {
			_cMana = _bMana + GetCurrentValueModification ("mana");
		}
		_mVitality = _bVitality  + GetCurrentValueModification ("hitpoints");
		_mCondition = _bVitality + GetCurrentValueModification ("durability");

	}

	/*
	//Diese Funktion gibt wieder ob das Objekt versteckt ist(return bool) und zusätzliche Informatioen wie diese Tarnung aufgebaut ist.
	public override bool IsVisible (out int HideValue, out AttributModificationHelper.Modification[] UseEffects)
	{
		if (CMS == CreatureMovementStatus.Sneaking || GetCurrentValueModification ("invisibility") > 0) {
			HideValue = (int)this ["hide"] + (int)GetCurrentValueModification ("invisibility");
			List<AttributModificationHelper.Modification> result = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
				return obj.AttributeName == "hide";
			}).UsedModifications;
			result.AddRange (AttributeHelper.Find (delegate(AttributModificationHelper obj) {
				return obj.AttributeName == "invisibility";
			}).UsedModifications);
			UseEffects = result.ToArray ();
			return true;
		}
		HideValue = 0;
		UseEffects = null;
		return false;
	}*/
	



	//Diese Funktion dient zum Zugriff auf den HP-Wert oder so, gibt die Menge des angerichten schaden zurück. Heilungen. bzw Absorbtionen müssen an die RecieveHealing Funktion weitergegeben werden.	
	protected override float _recieveDamage (float Value, string Typ, IRPGSource Source)
	{
		if (Typ != "abilitycost") {
			//Apply Resistance
			Value-=this["Resistance_"+Typ];
			Value*=this["Resistance%_"+Typ];
		}
		if (Value > 0) {
			//Send Damage Messgae to Effects
			foreach(TEffect e in Effects)
				foreach(EffectScriptObject so in e.ScriptObjects)
					so.OnTakeDamage(ref Value,Typ,Source);

			_cHitpoints -= Value;
			if (_cHitpoints < 0) {
				Value = _cHitpoints / -10;
				_cVitality -= Value;
				_cHitpoints = 0;
			}
		
			return Value;
		} else if (Value < 0)
			return recieveHealing (Value,Source);
		return 0;


	}
	
	//Dient zum Verrechnen von Heilung mit beispielsweise Heilmodifikationen

	protected override float recieveHealing (float Value,IRPGSource Source)
	{
		//Send Triggers
		foreach(TEffect e in Effects)
			foreach(EffectScriptObject so in e.ScriptObjects)
				so.OnRecieveHealing(ref Value,Source);
		Value *= GetCurrentValueModification ("healingamplification");
		_cHitpoints = Mathf.Clamp (_cHitpoints + Value, 0, _cVitality);

		return Value;
	}

	public override void OnNetworkUpdate (NetworkMessage msg)
	{
		IDComponentUpdateMsg M = msg.ReadMessage<IDComponentUpdateMsg> ();
		if (M.updateType != 0 && M.updateType <= 10)
			base.OnNetworkUpdate (msg);
		else {
			switch(M.updateType){
			case 0:
				CompactCreature cC=FileHelper.deserializeObject<CompactCreature>(M.data);
				setupObjectByCompact(cC,true);
				break;
			}
		}
	}

	public override IDComponentUpdateMsg CreateInitialSetupMessage ()
	{
		IDComponentUpdateMsg result = new IDComponentUpdateMsg ();
		result.id = getID ();
		result.componentName = GetType ().ToString ();
		result.updateType = 0;
		CompactCreature obj = new CompactCreature ();
		createCompactCreature (obj);
		result.data=FileHelper.serializeObject<CompactCreature>(obj);
		return result;
	}

}
