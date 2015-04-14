using UnityEngine;
using System.Collections;

//Diese Klasse ist Hauptsächlich eine Hilfsklasse für Würfel und alles was damit zusammenhängt. Also auch Proben und konkurrierende Proben
public static class Dice {
	//Diese Funktion überprüft ob der Sender die Anforderungen des Content erfüllt
	public static bool CheckContentRequirements(Content c,RPGObject Sender,bool DeepSearch){
		return true;
	}
	/**
	 * \brief Diese Funktion macht einen regülaren Check gegen eine beliebiege Anzahl an Limits
	 * 
	 * \param Pool Punkte zum Ausgleichen oder bei negativen Werten, die ausgeglichen werden
	 * \param IsCritical Gibt an ob es sich um ein kritisches Ergebnis handelt. Anhand von Result erkennt man ob positiv oder negativ.
	 * \param Limits Die Einzelnen Limits, gegen die man würfelt. Differenzen werden mit dem Pool verechnet
	 * \return Auskunft ob der Check gelungen ist
	 * 
	 * 
	 */
	public static bool RollCheck(int Pool, out bool IsCritical, params int[] Limits){
		bool failed = false;
		int criticalcounter = 0;//Wert der die critical result zählt.
		foreach (int l in Limits) {
			int r=Random.Range(1,21);
			if(r==1)
				criticalcounter++;
			else
			if(r==20)
				criticalcounter--;
			else{
			if(r>l)
				if(Pool>=r-l)
					Pool-=r-l;
					else
						failed=true;
			else
					failed=true;
				
			}
			}
		IsCritical = criticalcounter != 0;
		if (criticalcounter < 0 || failed)
			return false;
		else
			return true;
	}
		
	public static bool RollCheck(int SituationPool, ref int Bonus,out bool IsCritical, params int[] Limits){
		int Pool = SituationPool + Bonus;
		int criticalcounter = 0;
		bool failed = false;
		int overall = 0;
		foreach (int l in Limits) {
			int r=Random.Range(1,21);
			if(r==1)
				criticalcounter++;
			else
				if(r==20)
					criticalcounter--;
			else{
				if(l<r){
					//Prüfen ob Probe aufgeglichen werden kann
					if(Pool>r-l)
						Pool-=r-l;
					else
						failed=true;
				}else{
					overall+=l-r;
				}
			}
		}
		IsCritical = criticalcounter != 0;
			Pool += overall;
		if (Pool > Bonus)
			Pool = Bonus;
			Bonus = Pool;
		if (failed || criticalcounter < 0)
			return false;
		else
			return true;


	}


}
