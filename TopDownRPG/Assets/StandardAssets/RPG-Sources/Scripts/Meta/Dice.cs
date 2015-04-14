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
	 * \param Pool In:Pool an Punkten die Ausgeglichen werden müssen oder anders herum. Out: Anzahl der Punkte die Übrig sind oder die nicht ausgeglichen werden konnten
	 * \param IsCritical Gibt an ob es sich um ein kritisches Ergebnis handelt. Anhand von Result erkennt man ob positiv oder negativ.
	 * \param Limits Die Einzelnen Limits, gegen die man würfelt. Differenzen werden mit dem Pool verechnet
	 * \return Auskunft ob der Check gelungen ist
	 * 
	 * 
	 */
	public static bool RollCheck(ref int Pool, out bool IsCritical, params int[] Limits){
		int bonus = 0;
		bool failed = false;
		int criticalcounter = 0;//Wert der die critical result zählt.
		foreach (int l in Limits) {
			int r=Random.Range(1,21);
			if(r==1)
				criticalcounter++;
			if(r==20)
				criticalcounter--;
			if(r>l){
				if(Pool>=r-l)
					Pool-=r-l;
			else{
					failed=true;
				}
			}else{
				if(Pool>0)
				bonus+=l-r;
			}
			}
		IsCritical = criticalcounter != 0;
		if (Pool > 0)
			Pool += bonus;
		if (criticalcounter < 0 || failed)
			return false;
		else
			return true;
	}
		


}
