using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//Jedes Objekt, dass nicht zur Statischen Umgebung gehört ist ein ID-Objekt. Dieses Verwaltet die IDComponents, welche beispielsweise Aussehen() oder Verhalten(Controller) verwalten und so den Datenverkehr reduzieren.
public class IDObject : NetworkBehaviour {
	[SyncVar]
	private string _id;
	private bool isToBeSaved;
	public string id{
		get{return _id;}
	}

	List<IDComponent> components=new List<IDComponent>();

	//Wenn das Objekt nicht aus einem Verzeichnes geladen wurde, wird diese Funktion das Objekt im ID-System registrieren.
	[Command]
	public void Cmd_registerObject(){
		if (_id == "")
			_id=IDService.getUniqueID ();
	}


	List<NetworkConnection> additionalObservers=new List<NetworkConnection>();

	//Wenn das Objekt endgültig aus der Welt verschwindet soll, muss diese Funktion aufgerufen.
	public void unregisterObject(){
		if (base.isClient)
			return;
		WorldGrid.instance.main.unregister (this);
		IDService.freeID (_id);
		isToBeSaved = false;
		NetworkServer.Destroy (gameObject);
	}

	void LateUpdate(){
		if(base.isServer)
			updateGrid ();
	}

	Vector3 oldGridPosition;//=new Vector3(Mathf.Floor(transform.position.x/10),Mathf.Floor(transform.position.y/10),Mathf.Floor(transform.position.z/10));
	//called in LateUpdate und sortiert das Objekt in das WorldGrid ein.
	protected void updateGrid(){
		Vector3 currentGridPosition=new Vector3(Mathf.Floor(transform.position.x/10),Mathf.Floor(transform.position.y/10),Mathf.Floor(transform.position.z/10));
		if (oldGridPosition != currentGridPosition) {
			WorldGrid.instance.baseGrid[(int)oldGridPosition.x,(int)oldGridPosition.z,(int)oldGridPosition.y].unregister(this);
			WorldGrid.instance.baseGrid[(int)currentGridPosition.x,(int)currentGridPosition.z,(int)currentGridPosition.y].register(this);
		}
		oldGridPosition = currentGridPosition;
		NetworkWriter w = new NetworkWriter ();
	}


	public void loadFromFile(string ID){
		if (base.isClient)
			return;
		//Öffne das Verzeichnis des IDObjekts und generiere daraus die Components
		string[] neededComponents=new string[0];
		//(System.Activator("","Test",new string[]{id}) as IDComponent)
		foreach (string cs in neededComponents) {
			gameObject.AddComponent(System.Type.GetType(cs));
			components.Add(gameObject.GetComponent(cs) as IDComponent);
		}
	}

	public override void OnNetworkDestroy ()
	{
		base.OnNetworkDestroy ();
		if (!isToBeSaved||base.isClient)
			return;
		saveToFile ();
	}

	public void saveToFile(){
		//Lege einen Ordner mit dem Namen der ID an, falls notwendig lösche bestehenden Datein in diesem.
		//Lege eine Datei mit dem Namen "identity.obj" an, wo die Typ-Namen aller Componenten abgelegt sind
		//Deserialisiere alle Componenten auf den Pfad dieses Ordners
	}

}
