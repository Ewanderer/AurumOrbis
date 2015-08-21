using UnityEngine;
using System.Collections;

//Basisklasse für alle Aktionsklassen, die durch die Steuerung generiert werden können
public abstract class Executable
{
	protected string _name;
	public string name{
		get{return _name;}
	}

	protected float _executionDuration;//Falls die Aktion standardmäßig längere Zeit benötigt, z.B. Zauber, dann blockiert, wenn 0 gilt, dass die Aktion nach Ende der Initative-Runde beendet wird.
	public float executionDuration{
		get{return _executionDuration;}
	}
	protected bool _isInterruptableByOwner;//Ist dieser Wert wahr, kann die Aktion nicht abgebrochen/ersetzt werden, auch wenn dieser ungültig geworden ist.
	public bool iIBO{
		get{return _isInterruptableByOwner;}
	}



	//Diese Funktion wird aufgerufen, sobald diese Aktion ausgewählt wurde(Beginn der Animation, z.B. Ausholen)
	public abstract void Invoke(int Phase,Transform Target=null);
	public abstract void Invoke(int Phase,Vector3 t);
	//Diese Funktion wird aufgerufen, sofern die Aktion gewechselt wird. Diese Funktion räumt also hinter sich auf
	public abstract void StopInvoke();

	//Sobald das Aktionsmeter gefüllt ist, bzw. ihre executionDuratin absolviert wurde, wird diese Funktion mit der Angabe, in welcher Phase diese Aktion gewählt wurde 
	public abstract void Execute();
}

