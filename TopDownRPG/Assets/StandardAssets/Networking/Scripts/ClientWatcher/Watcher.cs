using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;

//Dieser Skript verwaltet das Laden von Spielfigur, sowie der Erstellen der Statischen Umgebung dieser und dem Abrufen der zugehörigen IDObjekte

enum networkState{
	error=-1,
	login=0,
	charselection=1,
	createcharacter=2,
	playing=3
};

public class Watcher : NetworkBehaviour {



	public static Watcher instance;


	networkState currentState=networkState.login;

	//Da nicht immer alle Objekte sichtbar sind, fragt der Watcher bei referenzAnfragen den Server ggf. um das Laden des entsprechenden Objekts zu Zugriffszwecken.
	public IDObject getIDObject(string ID){
		return null;
	}

	// Use this for initialization
	void Start () {


	}

	void OnLoginAnswer(NetworkMessage msg){
		LoginAnswerMsg answer = msg.ReadMessage<LoginAnswerMsg> ();
		avbCharNames = answer.characterNames;
		avbCharIDs = answer.characterIDs;
		accIdentity = answer.Accountidentity;
		currentState = networkState.charselection;
	}

	void OnSystemError(NetworkMessage msg){
		string errormsg = msg.ReadMessage<StringMessage> ().value;
		Debug.Log (errormsg);
		//Füge Methode zur Darstellung des Fehlers im Fenster an...
	}

	void Update(){
		
	}

	string inputName="";
	string inputPassword="";


	int accIdentity;
	string[] avbCharNames;
	string[] avbCharIDs;


	[Command]
	void Cmd_login(string name,string password,GameObject player){
		AccountService.instance.Login (name, password,player);
	}

	[Command]
	void Cmd_register(string name,string password,GameObject player){
		AccountService.instance.Register (name, password, player);
	}

	[Command]
	void Cmd_requestVisibilits (string ID, GameObject player){
		//NetworkIdentity nId;

		//nId.RebuildObservers (false);
	}


	public static RPGObject getReferenceObject(string ID){

		RPGObject result;
		List<RPGObject> objs = new List<RPGObject>(GameObject.FindObjectsOfType<RPGObject> ());
		if ((result = objs.Find (delegate(RPGObject obj) {
			return obj.getID () == ID;
		})) != default(RPGObject)) 
			return result;
		 else {
			Watcher.instance.Cmd_requestVisibilits (ID, Watcher.instance.gameObject);
			return getReferenceObject(ID);
		}
	}

	void OnGUI(){
		if(isLocalPlayer)
		switch (currentState) {
		case networkState.login:
			GUILayout.Label("Willkommen in Aurum Orbis");
			GUILayout.Label("Benutzername:");
			inputName=GUILayout.TextField(inputName);
			GUILayout.Label("Passwort:");
			inputPassword=GUILayout.TextField(inputPassword);
			if(GUILayout.Button("Login")){
				Cmd_login(inputName,inputPassword,gameObject);
			}
			if(GUILayout.Button("Register"))
				Cmd_register(inputName,inputPassword,gameObject);
			break;
		case networkState.charselection:
			GUILayout.Box("Charakterauswahl");
			int charsel=-1;
			charsel=GUILayout.SelectionGrid(charsel,avbCharNames,1);
			if(charsel!=-1){

			}
			break;
		}
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		MyNetworkManager.client.RegisterHandler (MyMsgType.LoginAnswer, OnLoginAnswer);
		foreach(Camera c in Camera.allCameras)
			c.tag="Untagged";
		//gameObject.AddComponent<Camera> ();
		gameObject.AddComponent<FlareLayer> ();
		gameObject.AddComponent<GUILayer> ();
		gameObject.tag="MainCamera";
		if (base.isClient)
			instance = this;

	}

	public void checkGridPoints(){
		foreach (GridPoint gp in GameObject.FindObjectsOfType<GridPoint>()) {
			//Logge bei allen "sichtbaren" GridPoints ein
			//Logge bei allen "nicht-sichtbaren" GridPoints aus
		}
	}



}
