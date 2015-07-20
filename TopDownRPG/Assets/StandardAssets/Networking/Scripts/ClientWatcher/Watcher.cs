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

public class Watcher : MonoBehaviour {



	public static Watcher instance;


	networkState currentState=networkState.login;

	//Da nicht immer alle Objekte sichtbar sind, fragt der Watcher bei referenzAnfragen den Server ggf. um das Laden des entsprechenden Objekts zu Zugriffszwecken.
	public IDObject getIDObject(string ID){
		return null;
	}

	// Use this for initialization
	void Start () {
		MyNetworkManager.client.RegisterHandler (MyMsgType.LoginAnswer, OnLoginAnswer);
		instance = this;
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

	public static RPGObject getReferenceObject(string ID){

		RPGObject result;
		List<RPGObject> objs = new List<RPGObject>(GameObject.FindObjectsOfType<RPGObject> ());
		if ((result = objs.Find (delegate(RPGObject obj) {
			return obj.getID () == ID;
		})) != default(RPGObject)) 
			return result;
		 else {
			MyNetworkManager.client.Send(MyMsgType.RequestVisibility,new StringMessage(ID));
			return getReferenceObject(ID);
		}
	}

	void OnGUI(){
		switch (currentState) {
		case networkState.login:
			GUILayout.Label("Willkommen in Aurum Orbis");
			GUILayout.Label("Benutzername:");
			inputName=GUILayout.TextField(inputName);
			GUILayout.Label("Passwort:");
			inputPassword=GUILayout.TextField(inputPassword);
			if(GUILayout.Button("Login"))
				MyNetworkManager.client.Send(MyMsgType.LoginRequest,new AccountCredentialMsg(inputName,inputPassword));
			if(GUILayout.Button("Register"))
				MyNetworkManager.client.Send(MyMsgType.NewAccountRequest,new AccountCredentialMsg(inputName,inputPassword));
			break;
		case networkState.charselection:
			GUILayout.Box("Charakterauswahl");
			int charsel=-1;
			charsel=GUILayout.SelectionGrid(charsel,avbCharNames,1);
			if(charsel!=-1)
				MyNetworkManager.client.Send(MyMsgType.HookChar,new StringMessage(avbCharIDs[charsel]));
			break;
		}
	}

}
