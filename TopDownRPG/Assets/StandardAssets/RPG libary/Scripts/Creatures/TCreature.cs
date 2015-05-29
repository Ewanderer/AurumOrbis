using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Diese Klasse repräsentiert den Charakterbogen und dient dem Zusammenfassen der Auswirkung aller Statuseffekte und Ausrüstungseffekte.

public class TCreature : RPGObject
{

	public override List<TEffect> Effects {
		get {
			List<TEffect> result = new List<TEffect> ();
			result.AddRange (cEffects);
			foreach (EquipmentSlot eq in Equipment)
				result.AddRange (eq.SlottedItem.Effects);
			return result.FindAll (delegate(TEffect obj) {
				return !obj.IsSupressed;
			});
		}
	}
	//Charakter-Statistiken

	//Primäre Attribute

	//Felder für die Basiswerte aller Primären Attribute, können nur durch "Training"/LvL-Up verändert werden
	int bStrength;
	int bCourage;
	int bAgility;
	int bPrestidigitation;
	int bConstitution;
	int bMetabolism;
	int bIntelligence;
	int bWisdom;
	int bCharisma;
	int bAppearance;
	//Felder für die durch Effekte modifizierten Attribute
	int cStrength;
	int cCourage;
	int cAgility;
	int cPrestidigitation;
	int cConstitution;
	int cMetabolism;
	int cIntelligence;
	int cWisdom;
	int cCharisma;
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

	float _bHitpoints;//Maximale Gesunheit, Grundwert
	float _mHitpoints;//Maximale Gesundheit, Endwert.
	float _cHitpoints;//Aktuelle Gesundheit wenn sie auf 0 fällt stirbt der Char, können nicht regeneriert werden solange nicht aller Schmerz wiederhergestellt wurde und auch benötigt man ärtzliche Behandlung.
	float _cPain;//Eine Art Schutz gegen Schwere Verwundungen. Wird durch Rast, Heilmittelchen oder einfache Heilzauber geschaffen. 

	float _bDurability;//Die Gesamtasudauer der Figur,Grundwert
	float _mDurability;//Die Gesamtausdauer der Figur, Endwert
	float _cExhaustion;//Der Grad der Erschöpfung durch langanhaltende Belastung(Tragen Schwerer Rüstung, Schlafmangel). Stellt auch die Maximale Grenze für Stamina-Regeneration da.
	float _cStamina;//Wird durch Kurzeitige Körperliche Aktivität benötigt

	float _bMana;//Maximaler Manapool, wenn 0 können keine anderen Effekte die Mana steigern greifen.
	float _mMana;
	float _cMana;


	//Öffentliche Felder für Sekundär-Attribute

	public float mHitpoints {
		get{ return _mHitpoints;}
	}

	public float cHitpoints {
		get{ return _cHitpoints;}
	}

	public float cPain {
		get{ return _cPain;}
	}

	public float mHealth {
		get{ return _mHitpoints * 11;}
	}

	/*
	 * Dieses Feld dient zur größeren Vedeutlicherung der Gesamt HP.
	 */
	public float cHealth {
		get{ return _cPain + cHitpoints * 10;}
	}

	public float mDurability {
		get{ return _mDurability;}
	}

	public float cExhaustion {
		get{ return _cStamina;}
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

	


	/*
	public bool this[string ValueName,int i]{
		get{return GetBBaseValue(ValueName.ToLower())&&GetCurrentValueModification(ValueName)>0;}
	}*/

	//Ausrüstung und Inventar

	public struct EquipmentSlot
	{
		public string Name;
		public TEquipment SlottedItem;
	}

	List<EquipmentSlot> _Equipment = new List<EquipmentSlot> ();
	List<TItem> _Inventory = new List<TItem> ();

	public List<EquipmentSlot> Equipment {
		get{ return _Equipment;}
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
	CreatureMovementStatus CMS;
	bool IsAlive;//Well Undeads are also alive :D



	//Diese Funktion wird bei jeder Veränderung von Equipment oder Effektliste neu aufgerufen
	protected override void UpdateStatistics ()
	{
		base.UpdateStatistics ();
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
		_mHitpoints = _bHitpoints + cConstitution * 10 + GetCurrentValueModification ("hitpoints");
		_mDurability = _bHitpoints + cConstitution * 100 + GetCurrentValueModification ("durability");

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
	public override float RecieveDamage (float Value, string Typ, IRPGSource Source)
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

			_cPain -= Value;
			if (_cPain < 0) {
				Value = _cPain / -10;
				_cHitpoints -= Value;
				_cPain = 0;
			}
		
			return Value;
		} else if (Value < 0)
			return RecieveHealing (Value,Source);
		return 0;


	}
	
	//Dient zum Verrechnen von Heilung mit beispielsweise Heilmodifikationen
	protected override float RecieveHealing (float Value,IRPGSource Source)
	{
		Value *= GetCurrentValueModification ("healingamplification");
		_cPain = Mathf.Clamp (_cPain + Value, 0, _cHitpoints);
		//Send Triggers
		foreach(TEffect e in Effects)
			foreach(EffectScriptObject so in e.ScriptObjects)
				so.OnRecieveHealing(ref Value,Source);
		return Value;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}
