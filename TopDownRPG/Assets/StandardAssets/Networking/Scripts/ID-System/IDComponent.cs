using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
[System.Serializable]
[RequireComponent(typeof(IDObject))]

public abstract class IDComponent:NetworkBehaviour,IDInterface {
	[System.NonSerialized]
	protected string _referenceID;

	public string getID(){
		return _referenceID;
	}

	[System.NonSerialized]
	protected IDObject referenceObject;


	//Gibt an ob beim Laden ein Update für diese Componenten gestartet werden muss wenn die Figur lädt...

	bool _doUpdateOnOpenBlock;
	public bool doUpdateOnOpenBlock{
		get{return this._doUpdateOnOpenBlock;}
	}

	public virtual void update(float timeSinceLastUpdate){}

	protected bool requireNetworkUpdate=false;
	protected bool requireUpdateOnAllClients=false;

	protected float NetworkUpdateRate=10;
	float timer;

	void Update(){
		if(requireNetworkUpdate&&isLocalPlayer&&isClient)
		if (timer <= Time.time) {
			timer=Time.time+NetworkUpdateRate;
			OnNetworkUpdate();
		}

		if(isLocalPlayer||requireUpdateOnAllClients)
			update (Time.deltaTime);
	}

	public virtual void initialize(){
		_referenceID = gameObject.GetComponent<IDObject> ().id;
		referenceObject = gameObject.GetComponent<IDObject> ();
	}

	void Start(){
		initialize();
	}

//	public abstract void applyChanges ();//Dieses Funktion dient nach Laden, bzw Erstellen aller Werte zur Umsetzung in Grafik etc.
	public abstract void serializeToFile(string FileName);
	public abstract void deserializeFromFile(string FileName);

	public virtual void OnNetworkUpdate(){
	}

}
