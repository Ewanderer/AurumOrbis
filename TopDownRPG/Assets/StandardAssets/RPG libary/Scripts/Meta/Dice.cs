using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum CheckResult
{
	ExtraordinaryFailure=-3,
	CriticalFailure=-2,	
	Failure=-1,				//One or more roll above Limit
	Succss=0,					//All rolls under Limit
	CriticalSuccess=1,		//Sucess with 50% of rolls a 1
	ExtraordinarySuccess=2,	//Sucess with 100% rolls of a 1	
};

public enum ComplexityClass
{
	Mundane=20,
	Heroic=40,
	Epic=60,
	Lengendary=80,
	Ultimate=100
}
;

//Diese Klasse ist Hauptsächlich eine Hilfsklasse für Würfel und alles was damit zusammenhängt. Also auch Proben und konkurrierende Proben
public static class Dice
{
	//Diese Funktion überprüft ob der Sender die Anforderungen des Content erfüllt
	public static bool CheckContentRequirements (Content c, RPGObject Sender, bool DeepSearch)
	{
		return true;
	}
	/**
	 * \brief Diese Funktion erlaubt einfache Proben und offene Proben.
	 * \param Pool Anzahl von Punkten, die man aufgrund seiner Fertigkeit erhält, der Restwert dient als Auskunft über den Grad des Erfolges/Misserfolges. Ist nach oben durch seinen Anfangswert beschränkt.
	 * \param Modification Die Erleichterung/Erschwerniss der Probe
	 * \param Limits Die einzelen Schranken, gegen die man würfel muss.
	 * 
	 */
	public static CheckResult RollCheck (ref int Pool, int Modification, int DiceSize, params int[] Limits)
	{
		int maxPool = Pool;
		bool IsFailed = false;
		if (Modification >= 0)
			Pool += Modification;
		int OneCount = 0;
		int TwentyCount = 0;
		foreach (int L in Limits) {
			int R = Random.Range (1, DiceSize + 1);
			if (R == 1)
				OneCount++;
			if (R == DiceSize)
				TwentyCount++;
			if (Modification < 0)
				R -= Modification;
			if (R <= L)
				continue;
			else {
				if (Pool > 0 && (Pool -= R - L) >= 0)
					continue;
				else
					IsFailed = true;
			}

		}
		if (Pool > maxPool)
			Pool = maxPool;
		if (Pool > DiceSize)
			Pool = DiceSize;
		if (OneCount == Limits.Length)
			return CheckResult.ExtraordinarySuccess;
		if (TwentyCount == Limits.Length)
			return CheckResult.ExtraordinaryFailure;
		if (!IsFailed) {
			if (OneCount > Mathf.Round (Limits.Length / 2))
				return CheckResult.CriticalSuccess;
			return CheckResult.Succss;
		}
		//Failed
		if (TwentyCount > Mathf.Round (Limits.Length / 2))
			return CheckResult.CriticalFailure;
		return CheckResult.Failure;

	}

	public static void InitializeDiceService ()
	{
		//Put all dem skills here !
		//Put all dem CompetitiveSkills Here
	}

	public static void ActionRoll (RPGObject Source, string TargetValueName, ComplexityClass Class, int Modifiaction, out CheckResult resultTyp, out int restPool)
	{
		string[] AttributNames = new string[3];
		float mod = 0f;
		//Bestimmung der zum Skill assoziierten Attribute

		RPGObject.Skill sk;
		if ((sk = (Source as RPGObject).Skills.Find (delegate(RPGObject.Skill obj) {
			return obj.SkillName == TargetValueName.Split ('-') [0];
		})) != default(TCreature.Skill)) {
			if (TargetValueName.Split ('-').Length > 1) {
				string[] s = TargetValueName.Split ('-');
				int tryies = -1;//Wir überprüfen zunächst ob die geforderte Fokusierung vorhanden ist.
				for (int i=0; i<sk.Focus.Length; i++) {
					if (sk.Focus [i] == s [1]) {
						AttributNames = sk.FocusAttributes [i];
						mod = 1;
						tryies++;
					}
				}
				if (tryies == -1) {//Fokus nicht Vorhanden
					//Krame Skill Helper Heraus um das Umrechungsverhältnis zu bestimmen
					SkillHelper sh;
					if ((sh = skh.Find (delegate(SkillHelper obj) {
						return obj.Skill == s [0];
					})) != default(SkillHelper)) {
						for (int i=0; i<sh.Substitution.Length; i++) {
							if (sh.Substitution [i].Split (',') [0] == s [1]) {
								mod = System.Convert.ToSingle (sh.Substitution [i].Split (',') [1]);
								AttributNames [0] = sk.Attribute1;
								AttributNames [1] = sk.Attribute2;
								AttributNames [2] = sk.Attribute3;
							}
						}
					} else
						Debug.Log ("SkillHelper-Gatter Unvollständing:" + TargetValueName);
				}
			} else {
				//Es wird nach dem allgemeinem Wert und seinen Attributen gefragt.
				mod = 1;
				AttributNames [0] = sk.Attribute1;
				AttributNames [1] = sk.Attribute2;
				AttributNames [2] = sk.Attribute3;
			}
		} else {
			//Krame SkillHelper zur Bestimmung der Attribute hervor
			SkillHelper sh;
			if ((sh = skh.Find (delegate(SkillHelper obj) {
				return obj.Skill == TargetValueName.Split ('-') [0];
			})) != default(SkillHelper)) {
				AttributNames = sh.associatedAttributes;
			}
		}
	
		//Beschaffung der Daten
		int bV = 0;
		int eV = 0;
		RPGObject.AttributModificationHelper.Modification[] m;
		RPGObject.AttributModificationHelper.Counter[] c;
		int[] AttributeValues = new int[3];
		AttributeValues [0] = (int)Source [AttributNames [0]];
		AttributeValues [1] = (int)Source [AttributNames [1]];
		AttributeValues [2] = (int)Source [AttributNames [2]];

		Source.CheckSkill (TargetValueName.Split ('-') [0], out bV, out eV, out m, out c);
		resultTyp = RollCheck (ref eV, Modifiaction, (int)Class, AttributeValues);
		//Ausführen des Rolls
		restPool = eV;

		//Auswertung
	}

	public static void ActionRoll (RPGObject Source, string TargetValueName, ComplexityClass Class, int Modifiaction, out CheckResult resultTyp)
	{
		string[] AttributNames = new string[3];
		float mod = 0f;

		//Bestimmung der zum Skill assoziierten Attribute

		RPGObject.Skill sk;
		if ((sk = (Source as RPGObject).Skills.Find (delegate(RPGObject.Skill obj) {
			return obj.SkillName == TargetValueName.Split ('-') [0];
		})) != default(RPGObject.Skill)) {
			if (TargetValueName.Split ('-').Length > 1) {
				string[] s = TargetValueName.Split ('-');
				int tryies = -1;//Wir überprüfen zunächst ob die geforderte Fokusierung vorhanden ist.
				for (int i=0; i<sk.Focus.Length; i++) {
					if (sk.Focus [i] == s [1]) {
						AttributNames = sk.FocusAttributes [i];
						mod = 1;
						tryies++;
					}
				}
				if (tryies == -1) {//Fokus nicht Vorhanden
					//Krame Skill Helper Heraus um das Umrechungsverhältnis zu bestimmen
					SkillHelper sh;
					if ((sh = skh.Find (delegate(SkillHelper obj) {
						return obj.Skill == s [0];
					})) != default(SkillHelper)) {
						for (int i=0; i<sh.Substitution.Length; i++) {
							if (sh.Substitution [i].Split (',') [0] == s [1]) {
								mod = System.Convert.ToSingle (sh.Substitution [i].Split (',') [1]);
								AttributNames [0] = sk.Attribute1;
								AttributNames [1] = sk.Attribute2;
								AttributNames [2] = sk.Attribute3;
							}
						}
					} else
						Debug.Log ("SkillHelper-Gatter Unvollständing:" + TargetValueName);
				}
			} else {
				//Es wird nach dem allgemeinem Wert und seinen Attributen gefragt.
				mod = 1;
				AttributNames [0] = sk.Attribute1;
				AttributNames [1] = sk.Attribute2;
				AttributNames [2] = sk.Attribute3;
			}
		} else {
			//Krame SkillHelper zur Bestimmung der Attribute hervor
			SkillHelper sh;
			if ((sh = skh.Find (delegate(SkillHelper obj) {
				return obj.Skill == TargetValueName.Split ('-') [0];
			})) != default(SkillHelper)) {
				AttributNames = sh.associatedAttributes;
			}
		}
	
		//Beschaffung der Daten
		int bV = 0;
		int eV = 0;
		RPGObject.AttributModificationHelper.Modification[] m;
		RPGObject.AttributModificationHelper.Counter[] c;
		int[] AttributeValues = new int[3];
		AttributeValues [0] = (int)Source [AttributNames [0]];
		AttributeValues [1] = (int)Source [AttributNames [1]];
		AttributeValues [2] = (int)Source [AttributNames [2]];
		Source.CheckSkill (TargetValueName.Split ('-') [0], out bV, out eV, out m, out c);
		if (eV < 0 && mod < 0)
			eV = (int)(eV * -mod);
		//Ausführen des Rolls
		resultTyp = RollCheck (ref eV, Modifiaction, (int)Class, AttributeValues);
		//Auswertung
	}

	public class SkillHelper
	{
		public string Skill;
		public string[] associatedAttributes;
		public string[] Counter;//Nacheinander werden die Gegenfertigkeiten aufgeführt, dabei sind sie in der Form: [SkillName]-[Spezialisierung],[Modifikator für den Rollwert]
		public string[] Substitution;

		public SkillHelper (string Name, string[] asso, string[] counter, string[] rep)
		{
			Skill = Name;
			associatedAttributes = asso;
			Counter = counter;
			Substitution = rep;
		}
	}

	public static List<SkillHelper> skh = new List<SkillHelper> ();

	public static void CompetitiveRoll (RPGObject source, string sourceValueName, RPGObject Target, ComplexityClass Class, int SMod, int TMod, out CheckResult resultTypSource, out CheckResult resultTypTarget, out int restPoolTarget)
	{
		//Bestimmung des TargetValueNames und der AttributValues mithilfe der SkillHelper.
		string targetValueName = "";
		int[] targetAttributeValues = new int[3];
		int[] sourceAttributeValues = new int[3];

		float mod = 0;
		SkillHelper sh;//With TargetValue Associated Skill
		bool targetStuffAssigned = false;

		if ((sh = skh.Find (delegate(SkillHelper obj) {
			return obj.Skill == sourceValueName;
		})) != default(SkillHelper)) {
			for (int i=0; i<sh.Counter.Length; i++) {
				mod = System.Convert.ToSingle (sh.Counter [i].Split (',') [1]);
				targetValueName = sh.Counter [i].Split (',') [0];

				RPGObject.Skill sk;
				if ((sk = (Target as RPGObject).Skills.Find (delegate(RPGObject.Skill obj) {//Untersuche ob die taregtKreatur über den Skill verfügt
					return obj.SkillName == sh.Counter [i].Split (',') [0].Split ('-') [0];
				})) != default(RPGObject.Skill)) {
					if (sh.Counter [i].Split (',') [0].Split ('-').Length > 1) {//Suche bei Spezialskills nach dem Fokus in der targetkreatur
						for (int ii=0; ii<sk.Focus.Length; ii++) {
							if (sk.Focus [ii] == sh.Counter [i].Split (',') [0].Split ('-') [1]) {
								targetAttributeValues [0] = (int)Target [sk.FocusAttributes [ii] [0]];
								targetAttributeValues [1] = (int)Target [sk.FocusAttributes [ii] [1]];
								targetAttributeValues [2] = (int)Target [sk.FocusAttributes [ii] [2]];
								targetStuffAssigned = true;
								break;
							}
						}
					} else {
						//targetKreaturen bringen für den gefundenen Skill ihre eigenen Attribute mit
						targetAttributeValues [0] = (int)Target [sk.Attribute1];
						targetAttributeValues [1] = (int)Target [sk.Attribute2];
						targetAttributeValues [2] = (int)Target [sk.Attribute3];
						targetStuffAssigned = true;
						break;
					}
				} else
					Debug.Log ("Skill-Gatter unvollständig:" + sh.Counter [i].Split (',') [0]);
			
			}
			//Im Fall das wir kein geeigneten targetSkill finden konnten nutzen wir die Standardwerte aus dem Helfer
			if (!targetStuffAssigned) {
				SkillHelper sh2;//Skill der Counterd
				if ((sh2 = skh.Find (delegate(SkillHelper obj) {
					return obj.Skill == targetValueName;
				})) != default(SkillHelper)) {
					targetAttributeValues [0] = (int)Target [sh2.associatedAttributes [0]];
					targetAttributeValues [1] = (int)Target [sh2.associatedAttributes [1]];
					targetAttributeValues [2] = (int)Target [sh2.associatedAttributes [2]];
					targetStuffAssigned = true;
				} else
					Debug.Log ("Skill-Gatter unvollständig:" + targetValueName);
			}


			//Beschaffung der Attributsschwelle für Source
			RPGObject.Skill skSource;
			if ((skSource = source.Skills.Find (delegate(RPGObject.Skill obj) {
				return obj.SkillName == sourceValueName.Split (',') [0].Split ('-') [0];
			})) != default(RPGObject.Skill)) {
				sourceAttributeValues [0] = (int)source [skSource.Attribute1];
				sourceAttributeValues [1] = (int)source [skSource.Attribute2];
				sourceAttributeValues [2] = (int)source [skSource.Attribute3];
				if (sourceValueName.Split (',') [0].Split ('-').Length > 1) {
					for (int i=0; i<skSource.Focus.Length; i++) {
						if (sourceValueName.Split (',') [0].Split ('-') [1] == skSource.Focus [i]) {
							sourceAttributeValues [0] = (int)source [skSource.FocusAttributes [i] [0]];
							sourceAttributeValues [1] = (int)source [skSource.FocusAttributes [i] [1]];
							sourceAttributeValues [2] = (int)source [skSource.FocusAttributes [i] [2]];
						}
					}

				}
			} else {
				SkillHelper shsource;
				if ((shsource = skh.Find (delegate(SkillHelper obj) {
					return obj.Skill == sourceValueName.Split (',') [0].Split ('-') [0];
				})) != default(SkillHelper)) {
					sourceAttributeValues [0] = (int)source [shsource.associatedAttributes [0]];
					sourceAttributeValues [1] = (int)source [shsource.associatedAttributes [1]];
					sourceAttributeValues [2] = (int)source [shsource.associatedAttributes [2]];
				} else
					Debug.Log ("Skill-Gatter unvollständig:" + sourceValueName);
			}

			//Beschaffung der Modifikationen, Counter und Basiswerte für Source und Target
			int sBV;
			int sEV;
			int tBV;
			int tEV;
			RPGObject.AttributModificationHelper.Modification[] sM;
			RPGObject.AttributModificationHelper.Modification[] tM;
			RPGObject.AttributModificationHelper.Counter[] sC;
			RPGObject.AttributModificationHelper.Counter[] tC;
			source.CheckSkill (sourceValueName, out sBV, out sEV, out sM, out sC);
			Target.CheckSkill (targetValueName, out tBV, out tEV, out tM, out tC);
			//Gegenrechnen der Counter und Modificatoren für die Source
			if (tC.Length > 0) {
				//Gegenrechnen von sM
				foreach (RPGObject.AttributModificationHelper.Counter cC in tC) {
					for (int i=0; i<sM.Length; i++) {
						if (cC.SourceTyp [0] == '§') {
							string helper = cC.SourceTyp.TrimStart ('§');

							if (sM [i].SourceEffect.GeneralOrder < cC.Order && (sM [i].SourceEffect.GeneralCategory == helper || sM [i].SourceEffect.Tags.Contains (helper)))
								sM [i] = null;
						
						} else {
							if (sM [i].Order < cC.Order && sM [i].SourceType == cC.SourceTyp)
								sM [i] = null;
						}
					}
				}
				//Zusammenrechnen aller sM auf den sBV und in sEV legen.
				sEV = sBV;
				foreach (RPGObject.AttributModificationHelper.Modification cM in sM) {
					sEV +=(int)cM.Value;
				}
			}
			//Ausführung des ersten Rolls
			resultTypSource = RollCheck (ref sEV, SMod, (int)Class, sourceAttributeValues);
			if (resultTypSource >= 0) {
				//Wenn der erste Roll nicht scheitert, müssen wir auch noch eine Probe für das Ziel ablegen.
				if (sC.Length > 0) {
					//Gegenrechnen von tM
					foreach (RPGObject.AttributModificationHelper.Counter cC in sC) {
						for (int i=0; i<tM.Length; i++) {
							if (cC.SourceTyp [0] == '§') {
								string helper = cC.SourceTyp.TrimStart ('§');
								if (tM [i].SourceEffect.GeneralOrder < cC.Order && (tM [i].SourceEffect.GeneralCategory == helper || tM [i].SourceEffect.Tags.Contains (helper)))
									tM [i] = null;
								
							} else {
								if (tM [i].Order < cC.Order && tM [i].SourceType == cC.SourceTyp)
									tM [i] = null;
							}
						}
					}
					//Zusammenrechnen aller tM auf den tBV und in tEV legen.
					tEV = tBV;
					foreach (RPGObject.AttributModificationHelper.Modification cM in tM) {
						tEV +=(int) cM.Value;
					}
				}
				tEV = (int)(tEV * mod);
				//Ausführen des zweiten Rolls
				resultTypTarget = RollCheck (ref tEV, TMod - sEV, (int)Class, targetAttributeValues);
				restPoolTarget = tEV;
				if ((int)resultTypTarget > (int)resultTypSource && resultTypSource >= 0)
					resultTypSource = CheckResult.Failure;
				
			} else {
				resultTypTarget = CheckResult.Succss;
				restPoolTarget = 0;
			}

		} else
			Debug.LogError ("Skill-Gatter unvollständig:" + sourceValueName);
		//Create Impossible Outcome
		resultTypSource = CheckResult.Succss;
		resultTypTarget = CheckResult.Succss;
		restPoolTarget = 0;

	}
	/*
	public static void AttackRoll (RPGObject Agressor, RPGObject Defendor, ComplexityClass Class, int SMod, int TMod, out CheckResult resultTypAgressor, out CheckResult resultTypDefendor)
	{

	}*/

}
