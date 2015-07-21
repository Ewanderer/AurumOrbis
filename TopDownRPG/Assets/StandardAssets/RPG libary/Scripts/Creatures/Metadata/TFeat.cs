using UnityEngine;
using System.Collections;

public class TFeat {
	public string Name;
	public string ShortDescription;
	public string LongDescription;
	//Kostenwerte. Für Kosten am aktuellen Wert der Figur, gehen wir von dem Wert nach Abzug von Asoluten und Maximalen Kosten. 
	public float cHealthAbsoulute;
	public float cHealthMax;
	public float cHealthCurrent;

	public float cStaminaAbsolute;
	public float cStaminaMax;
	public float cStaminaCurrent;

	public float cManaAbsolute;
	public float cManaMax;
	public float cManaCurrent;

	public string[] Script;


	/**
	 * Funktion die vor dem Ausführen des Skripts die Verfügbarkeit der Kostenkomponente überprüft und auch anwendet.
	 */
	bool ApplyCost(TCreature c){
		//Erst auf Verfügbarkeit der Ressourcen testen
		if ((cHealthAbsoulute > 0||cHealthMax>0)&&(c.cHealth<c.mHealth*cHealthMax+cHealthAbsoulute||c.cHealth<1))
			return false;
		if ((cStaminaAbsolute > 0||cStaminaMax>0)&&(c.cStamina<c.cExhaustion*cStaminaMax+cStaminaAbsolute||c.cStamina<1))
			return false;
		if ((cManaAbsolute > 0 || cManaMax>0 || cManaCurrent>0) && (c.mMana <= 0 || c.cMana < 1 || c.mMana < cManaAbsolute + c.mMana * cManaMax + cManaAbsolute))
			return false;
		//Geforderte Ressourcen abziehen.



		return true;
	}

	public bool Activate(RPGObject Target,TCreature Invoker){
		if (ApplyCost(Invoker)) {
			return true;
		}
		return false;
	}

	public bool Activate(Vector3 Target,TCreature Invoker){
		if (ApplyCost(Invoker)) {
			return true;
		}
		return false;
	}
}
