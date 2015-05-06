using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InteractiveObject : RPGObject
{	

	/**Hilfsklasse um Standardwerte zu verwalten*/
	public abstract class ValueHelper
	{
		public string ValueName;/**<Name des Werts*/
	}
	
	/**Spezialiesierte Hilfsklasse um Numerische(float) Werte zu speichern.*/
	public class NumericValueHelper:ValueHelper
	{
		public float Value;/**<Naja der dem Namen zugeordnete Standard-Wert*/
	}
	/**Spezialisierte Hilfsklasse um boolsche Werte zu speichern*/
	public class booleanValueHelper:ValueHelper
	{
		public bool Value;
	}
	/**Liste zur Verwaltung aller Standardwerte */
	protected List<ValueHelper> bValues = new List<ValueHelper> ();
	
	/**
	 * \brief Sucht den Standardwert aus der Liste bValues raus
	 * \param Name Name des gesuchten Standardwerts
	 * \return Wert
	 */
	protected float GetNBaseValue (string Name)
	{
		if (bValues.Exists (delegate(ValueHelper obj) {
			return (obj is NumericValueHelper) && obj.ValueName == Name;
		}))
			return (bValues.Find (delegate(ValueHelper obj) {
				return (obj is NumericValueHelper) && obj.ValueName == Name;
			}) as NumericValueHelper).Value;
		else
			return 0;
	}
	
	/**
	 * \brief Sucht den Standardwert aus der Liste bValues raus
	 * \param Name Name des gesuchten Standardwerts
	 * \return Wert
	 */
	protected bool GetBBaseValue (string Name)
	{
		if (bValues.Exists (delegate(ValueHelper obj) {
			return (obj is booleanValueHelper) && obj.ValueName == Name;
		}))
			return (bValues.Find (delegate(ValueHelper obj) {
				return (obj is booleanValueHelper) && obj.ValueName == Name;
			})as booleanValueHelper).Value;
		else
			return false;
	}

	public override float this [string ValueName] {
		get {
			float rvalue = 0;
			if ((rvalue += GetNBaseValue(ValueName)) != 0 || (rvalue += base [ValueName]) != 0)
				return rvalue;
			if(GetBBaseValue(ValueName))
				return 1;
			return base [ValueName];
		}
	}

	//Funktionsrümpfe
	
	//Diese Funktion gibt wieder ob das Objekt versteckt ist(return bool) und zusätzliche Informatioen wie diese Tarnung aufgebaut ist.
	public override bool IsVisible (out int HideValue, out TEffect[] UseEffects)
	{
		HideValue = 0;
		float r=0;
		UseEffects=new TEffect[0];
		AttributModificationHelper ahh;
		AttributModificationHelper ahi;
		if ((r += GetNBaseValue ("hide")) > 0 || ((ahh = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
			return obj.AttributeName == "hide";
		})) != default(AttributModificationHelper) && ((r += ahh.OverallModification) > 0)) || GetBBaseValue ("invisibility") || ((ahi = AttributeHelper.Find (delegate(AttributModificationHelper obj) {
			return obj.AttributeName == "invisibility";
		})) != default(AttributModificationHelper) && ahi.OverallModification > 0)) {
			List<TEffect> eff=new List<TEffect>();
			eff.AddRange(ahh.UsedModifications);
			eff.AddRange(ahi.UsedModifications);
			HideValue=(int)r;
			UseEffects=eff.ToArray();
			return true;
		}
		return false;
	}
	
	//Das Objekt macht Auskunft über die Verfügbarkeit einer Eigenschaft und bei numerischen Werten Auskunft über die Höhe des gefragten bestimmten Wertes. Dabei werden alle Informationen wie sich der Wert zusammengesetzt mitgegeben.
	public override bool CheckValue (string NameOfValue, out int BaseValue, out int EndValue, out TEffect[] UseEffects)
	{
		EndValue = 0;
		float r = GetNBaseValue (NameOfValue);

		if (GetBBaseValue (NameOfValue))
			r += 1;
		BaseValue=(int)r;
		AttributModificationHelper amh;
		UseEffects=new TEffect[0];
		if ((amh=AttributeHelper.Find(delegate(AttributModificationHelper obj) {
			return obj.AttributeName==NameOfValue;
	}))!=default(AttributModificationHelper)&&(EndValue=(int)(r+amh.OverallModification))>0) {
			UseEffects=amh.UsedModifications.ToArray();
			return true;
		}
		return false;
	}

	public override float RecieveDamage (float Value, string Typ)
	{
		//Apply Resistance
		if (Value > 0) {
			//Send DamageMessage
		} else
			if (Value < 0)
				return RecieveHealing (Value);
		return Value;
	}

	protected override float RecieveHealing (float Value)
	{
		Value *= GetCurrentValueModification ("healingamplification");
		//Send Triggers
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

