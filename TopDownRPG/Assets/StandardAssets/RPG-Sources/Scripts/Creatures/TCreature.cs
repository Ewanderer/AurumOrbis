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

	//Felder für die Basiswerte aller Primären Attribute, können nur durch "Training"/LvL-Up verändert werden
	int bStrength;
	int bDexterity;
	int bConstitution;
	int bMetabolism;
	int bIntelligence;
	int bWisdom;
	int bCharisma;
	int bAppearance;
	//Felder für die durch Effekte modifizierten Attribute
	int cStrength;
	int cDexterity;
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

	public int Dexterity {
		get{ return cDexterity;}
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

	//Diese Funktion wird bei jeder Veränderung von Equipment oder Effektliste neu aufgerufen
	protected override void UpdateStatistics ()
	{
		base.UpdateStatistics ();
		//Berechne die current Values zur schnellerern Abgreifung
		cStrength = bStrength + (int)GetCurrentValueModification ("strength");
		cDexterity = bDexterity + (int)GetCurrentValueModification ("dexterity");
		cConstitution = bConstitution + (int)GetCurrentValueModification ("constitution");
		cMetabolism = bMetabolism + (int)GetCurrentValueModification ("metabolism");
		cIntelligence = bIntelligence + (int)GetCurrentValueModification ("intelligence");
		cWisdom = bWisdom + (int)GetCurrentValueModification ("wisdom");
		cCharisma = bCharisma + (int)GetCurrentValueModification ("charisma");
		cAppearance = bAppearance + (int)GetCurrentValueModification ("appearance");
	}


	//Diese Funktion gibt wieder ob das Objekt versteckt ist(return bool) und zusätzliche Informatioen wie diese Tarnung aufgebaut ist.
	public override bool IsVisible(out int HideValue,out TEffect[] UseEffects){

	}
	
	//Das Objekt macht Auskunft über die Verfügbarkeit einer Eigenschaft und bei numerischen Werten Auskunft über die Höhe des gefragten bestimmten Wertes. Dabei werden alle Informationen wie sich der Wert zusammengesetzt mitgegeben.
	public override bool CheckValue (string NameOfValue, out int BaseValue, out int EndValue, out TEffect[] UseEffects){

	}
	
	//Diese Funktion dient zum Zugriff auf den HP-Wert oder so, gibt die Menge des angerichten schaden zurück. Heilungen. bzw Absorbtionen müssen an die RecieveHealing Funktion weitergegeben werden.	
	public override float RecieveDamage (float Value, string Typ){

	}
	
	//Dient zum Verrechnen von Heilung mit beispielsweise Heilmodifikationen
	protected override float RecieveHealing(float Value){

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
