using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[System.Serializable]
[RequireComponent(typeof(IDObject))]

public abstract class IDComponent:NetworkBehaviour {

	protected string referenceID;
	protected IDObject referenceObject;


	public virtual void update(float timeSinceLastUpdate){}

	void Update(){
		if (isLocalPlayer)
			update (Time.deltaTime);
	}

	public virtual void initialize(){
		referenceID = gameObject.GetComponent<IDObject> ().id;
		referenceObject = gameObject.GetComponent<IDObject> ();
	}

	void Start(){
		initialize();
	}

//	public abstract void applyChanges ();//Dieses Funktion dient nach Laden, bzw Erstellen aller Werte zur Umsetzung in Grafik etc.
	public abstract void serializeToFile(string FileName);
	public abstract void deserializeFromFile(string FileName);

}
