using UnityEngine;
using System.Collections;

public enum CheckResult{
	ExtraordinaryFailure=-3,
	CriticalFailure=-2,	
	Failure=-1,				//One or more roll above Limit
	Succss=0,					//All rolls under Limit
	CriticalSuccess=1,		//Sucess with 50% of rolls a 1
	ExtraordinarySuccess=2,	//Sucess with 100% rolls of a 1	
};

//Diese Klasse ist Hauptsächlich eine Hilfsklasse für Würfel und alles was damit zusammenhängt. Also auch Proben und konkurrierende Proben
public static class Dice {
	//Diese Funktion überprüft ob der Sender die Anforderungen des Content erfüllt
	public static bool CheckContentRequirements(Content c,RPGObject Sender,bool DeepSearch){
		return true;
	}
	/**
	 * \brief Diese Funktion erlaubt einfache Proben und offene Proben.
	 * \param Pool Anzahl von Punkten, die man aufgrund seiner Fertigkeit erhält, der Restwert dient als Auskunft über den Grad des Erfolges/Misserfolges. Ist nach oben durch seinen Anfangswert beschränkt.
	 * \param Modification Die Erleichterung/Erschwerniss der Probe
	 * \param Limits Die einzelen Schranken, gegen die man würfel muss.
	 * 
	 */
	public static CheckResult RollCheck(ref int Pool,int Modification, params int[] Limits){
		int maxPool = Pool;
		bool IsFailed = false;
		if (Modification >= 0)
			Pool += Modification;
		int OneCount = 0;
		int TwentyCount = 0;
		foreach (int L in Limits) {
			int R=Random.Range(1,21);
			if(R==1)
				OneCount++;
			if(R==20)
				TwentyCount++;
			if(Modification<0)
				R-=Modification;
			if(R<=L)
				continue;
			else{
				if(Pool>0&&(Pool-=R-L)>=0)
					continue;
				else
					IsFailed=true;
			}

		}
		if (Pool > maxPool)
			Pool = maxPool;
		if (OneCount == Limits.Length)
			return CheckResult.ExtraordinarySuccess;
		if (TwentyCount == Limits.Length)
			return CheckResult.ExtraordinaryFailure;
		if (!IsFailed) {
			if(OneCount>Mathf.Round(Limits.Length/2))
				return CheckResult.CriticalSuccess;
			return CheckResult.Succss;
		}
		//Failed
		if (TwentyCount > Mathf.Round (Limits.Length / 2))
			return CheckResult.CriticalFailure;
		return CheckResult.Failure;

	}

	/*
	 * \brief Diese Funktion verwaltet konkurierende Proben.
	 * \param Source Der Herausfoderer, sein Pool Modifikation für das Ziel
	 * \param Target Der Herausgeforderte
	 * \param SourceValueName Wert zur Identifizierung des vom Herausfoderer gefragten Eigenschaft.
	 * \param TargetValueName Wert zur "				"	"	Herausgefordertem gefragetn Eigenschaft.
	 * \param rSource Das Ergebnis der Probe des Herausfoders, an sich nur Interessant von Eo-Failure, zur Veringerung der Eigenschaft.
	 * \param rTarget Das Ergebnis der Probe des Herausgeforderten, sein Ergebniss ist entscheidend für die Bestimmung des Ausgangs.
	 * \param TargetBonus Der Rest-Pool des Herausgeforderten, um Auskunft über die Qualität seinen Ergebnisses zu erhalten.
	 * 
	 */

	public static void MakeCompetitiveRoll(RPGObject Source, RPGObject Target, string SourceValueName, string TargetValueName, out CheckResult rSource,out CheckResult rTarget,out int TargetBonus){

	}

}
