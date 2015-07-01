using UnityEngine;
using System.Collections;
[System.Serializable]
public abstract class IDComponent:System.Object {
	[System.Serializable]
	protected bool _doUpdateOnLoad;
	protected string referenceID;

	public bool doUpadteOnLoad{
		get{return _doUpdateOnLoad;}
	}

	public virtual void update(float timeSinceLastUpdate, GameObject self){}
	public abstract void applyChanges ();//Dieses Funktion dient nach Laden, bzw Erstellen aller Werte zur Umsetzung in Grafik etc.
	public abstract void serializeToFile(string FileName);
	public abstract void deserializeFromFile(string FileName);
	public IDComponent(string id){
		referenceID = id;
	}
}
