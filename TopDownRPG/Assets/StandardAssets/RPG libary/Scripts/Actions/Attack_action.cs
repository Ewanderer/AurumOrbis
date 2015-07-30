using UnityEngine;
using System.Collections;
//Nahkampfangriffe, Zauber, Angriffsfertigkeiten, eigentlich auch Buffs
public class Attack_Action : Executeable
{
	bool TargetMode;//True=flexible,false=static
	Transform flexibleTarget=null;
	Vector3 staticTarget=null;

	Vector3 Target{
		get{
			if(TargetMode)
				return flexibleTarget.position;
			return staticTarget;
		}
	}
}

