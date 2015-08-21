using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//Jedes Objekt, dass nicht zur Statischen Umgebung gehört ist ein ID-Objekt. Dieses Verwaltet die IDComponents, welche beispielsweise Aussehen() oder Verhalten(Controller) verwalten und so den Datenverkehr reduzieren.
public class IDObject : NetworkBehaviour,IDInterface {
	[SyncVar]
	private string _id;
	private bool isToBeSaved=true;

	public string id{
		get{return _id;}
	}

	public string getID(){
		return _id;
	}

	//Dieser Wert bei Spielern auf false gesetzt, wenn sie sich in einem Chunk befinden, der nicht komplett geladen wurde.
	bool loadingNetwork;
	//Dieser wird allgemein gesetzt wenn das Objekt sich in der Statischen Welt an einem noch nicht geladenem Fleckchen befindet.
	bool loadingWorld;

	bool CheckNetworkLoading(){
		//Überprüfe den Count der im Chunk des Spielers befindlichen Objekte mit dem von Laut Spieler in diesem Chunk gemeldeten, wenn weniger als 75%, dann lade...
		return true;//Fürs erste verzichten wir auf diese Maßnahme und werden sie erst bei sichtbaren Problemen einführen.
	}

	bool CheckWorldLoading(){
		//Überprüfe den Count der im Chunk des Spieler befindlichen Objekten und vergleiche mit der Angabe des WorldManagers. Wenn kleiner dann gilt das Objekt als ladend...
		return !StaticWorldManager.instance.usedWorld.Nodes[(int)oldGridPosition.x,(int)oldGridPosition.y,(int)oldGridPosition.z].isLoaded;;
	}

	//Während dieser Wert true ist, wird das gesamte Objekt eingefroren...
	public bool isLoading{
		get{return loadingNetwork||loadingWorld;}
	}

	void Update(){
		bool oldvalue = isLoading;
		if (isClient)
			loadingNetwork = CheckNetworkLoading ();
		else
			loadingNetwork = false;
		loadingWorld = CheckNetworkLoading ();
		if (oldvalue != isLoading) {
			if (isLoading) {
				foreach (MonoBehaviour mb in gameObject.GetComponents<MonoBehaviour>())
					if (mb != this)
						mb.enabled = false;
				foreach (GameObject go in gameObject.GetComponentsInChildren<GameObject>())
					go.SetActive (false);
			} else {
				foreach (MonoBehaviour mb in gameObject.GetComponents<MonoBehaviour>())
						mb.enabled = true;
				foreach (GameObject go in gameObject.GetComponentsInChildren<GameObject>())
					go.SetActive (true);
			}
		}
	}

	List<IDComponent> components=new List<IDComponent>();

	//Wenn das Objekt nicht aus einem Verzeichnes geladen wurde, wird diese Funktion das Objekt im ID-System registrieren.

	public void Cmd_registerObject(){
		if (isClient)
			return;
		if (_id == "")
			_id=IDService.getUniqueID ();
	}


	List<NetworkConnection> observerRequest=new List<NetworkConnection>();

	public override bool OnCheckObserver (NetworkConnection conn)
	{
		return (Vector3.Distance (conn.playerControllers [0].gameObject.GetComponent<Transform> ().position, transform.position) <= 20 || observerRequest.Contains(conn));
	}

	public override bool OnRebuildObservers (HashSet<NetworkConnection> observers, bool initialize)
	{
		foreach (NetworkConnection c in observerRequest)
			observers.Add (c);
		return base.OnRebuildObservers(observers,initialize);

	}

	List<NetworkConnection> additionalObservers=new List<NetworkConnection>();

	//Wenn das Objekt endgültig aus der Welt verschwindet soll, muss diese Funktion aufgerufen.

	public void Cmd_unregisterObject(){
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
			WorldGrid.instance.baseGrid[(int)oldGridPosition.x,(int)oldGridPosition.y,(int)oldGridPosition.z].unregister(this);
			WorldGrid.instance.baseGrid[(int)currentGridPosition.x,(int)currentGridPosition.y,(int)currentGridPosition.z].register(this);
			StaticWorldManager.unregisterIDObject(this,oldGridPosition);
			StaticWorldManager.registerIDObject(this,currentGridPosition);
		}
		oldGridPosition = currentGridPosition;
	}


	public void loadFromFile(string ID){
		if (base.isClient)
			return;
		//Öffne das Verzeichnis des IDObjekts und generiere daraus die Components
		string[] neededComponents=FileHelper.deserializeObject<string[]>(FileHelper.ReadFromFile("./server/objects/"+id+"/identity.obj")); 
		//(System.Activator("","Test",new string[]{id}) as IDComponent)
		foreach (string cs in neededComponents) {
			gameObject.AddComponent(System.Type.GetType(cs));
			components.Add(gameObject.GetComponent(cs) as IDComponent);
		}
	}

	void OnDestroy()
	{
		if (!isToBeSaved||base.isClient)
			return;
		saveToFile ();
	}

	public void saveToFile(){
		//Lege einen Ordner mit dem Namen der ID an, falls notwendig lösche bestehenden Datein in diesem.
		FileHelper.emptyPath ("./server/objects/" + id);
		List<string> componentNames = new List<string> ();
		foreach (IDComponent c in components) {
			componentNames.Add (c.GetType ().ToString ());
			//Deserialisiere alle Componenten auf den Pfad dieses Ordners
			c.serializeToFile("./server/objects/"+id+"/"+c.GetType ().ToString ()+".obj");
		}
		//Lege eine Datei mit dem Namen "identity.obj" an, wo die Typ-Namen aller Componenten abgelegt sind
		FileHelper.WriteToFile ("./server/objects/"+id+"/identity.obj",FileHelper.serializeObject<string[]>(componentNames.ToArray()));
	}

	//Diese Funktion wird nach Spawnen der Rohform auf den Clients und dem Laden, bzw. der Generierung des Objekt, alle ID-Componente zum Update aufzufordern. 
	public void distributeObjectStateToObservers(NetworkConnection player=null){
		if (isClient)
			return;
		List<IDComponentUpdateMsg> msgs = new List<IDComponentUpdateMsg> ();
		foreach (IDComponent comp in GetComponents<IDComponent>())
			msgs.Add (comp.CreateInitialSetupMessage());
		this.GetComponent<NetworkIdentity> ().RebuildObservers (true);
		foreach (NetworkConnection c in this.GetComponent<NetworkIdentity>().observers)
			foreach (IDComponentUpdateMsg msg in msgs)
				NetworkServer.SendToClient (c.connectionId, MyMsgType.IDComponentUpdateMessage, msg);
		if (player != null) {
			foreach (IDComponentUpdateMsg msg in msgs)
				NetworkServer.SendToClient(player.connectionId,MyMsgType.IDComponentUpdateMessage,msg);
		}
	}

}
