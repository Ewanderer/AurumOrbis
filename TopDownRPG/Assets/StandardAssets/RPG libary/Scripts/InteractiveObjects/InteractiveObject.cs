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

	float _cHP;
	float _mHP;

	public override float recieveDamage (float Value, string Typ, IRPGSource Source)
	{
		//Apply Resistance
		Value -= this["Resistance_" + Typ];
		Value *= this ["Resistance%_" + Typ];
		if (Value > 0) {
			foreach(TEffect e in Effects)
				foreach(EffectScriptObject so in e.ScriptObjects)
					so.OnTakeDamage(ref Value,Typ,Source);//Send DamageMessage
			_cHP-=Value;
			//Add Destruction
		} else
			if (Value < 0)
				return recieveHealing (Value,Source);
		return Value;
	}

	protected override float recieveHealing (float Value,IRPGSource Source)
	{
		Value *= GetCurrentValueModification ("healingamplification");
		//Send Triggers
		foreach(TEffect e in Effects)
			foreach(EffectScriptObject so in e.ScriptObjects)
				so.OnRecieveHealing(ref Value,Source);
		_cHP += Value;
		return Value;
	}

}

