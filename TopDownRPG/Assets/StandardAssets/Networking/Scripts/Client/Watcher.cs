using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking.NetworkSystem;

//Dieser Skript verwaltet das Laden von Spielfigur, sowie der Erstellen der Statischen Umgebung dieser und dem Abrufen der zugehörigen IDObjekte

enum networkState
{
	error=-1,
	login=0,
	charselection=1,
	createcharacter=2,
	playing=3
}
;

public class Watcher : MonoBehaviour
{



	public static Watcher instance;
	networkState currentState = networkState.login;
	GameObject playerObject;



	// Use this for initialization
	void Start ()
	{
		MyNetworkManager.client.RegisterHandler (MyMsgType.LoginAnswer, OnLoginAnswer);
		instance = this;
	}

	void OnLoginAnswer (NetworkMessage msg)
	{
		LoginAnswerMsg answer = msg.ReadMessage<LoginAnswerMsg> ();
		avbCharNames = answer.characterNames;
		avbCharIDs = answer.characterIDs;
		accIdentity = answer.Accountidentity;
		currentState = networkState.charselection;
	}


	void OnSystemError (NetworkMessage msg)
	{
		string errormsg = msg.ReadMessage<StringMessage> ().value;
		Debug.Log (errormsg);
		//Füge Methode zur Darstellung des Fehlers im Fenster an...
	}

	void Update ()
	{
		
	}

	string inputName = "";
	string inputPassword = "";
	int accIdentity;
	string[] avbCharNames;
	string[] avbCharIDs;

	void OnGUI ()
	{
		switch (currentState) {
		case networkState.login:
			GUILayout.Label ("Willkommen in Aurum Orbis");
			GUILayout.Label ("Benutzername:");
			inputName = GUILayout.TextField (inputName);
			GUILayout.Label ("Passwort:");
			inputPassword = GUILayout.TextField (inputPassword);
			if (GUILayout.Button ("Login"))
				MyNetworkManager.client.Send (MyMsgType.LoginRequest, new AccountCredentialMsg (inputName, inputPassword));
			if (GUILayout.Button ("Register"))
				MyNetworkManager.client.Send (MyMsgType.NewAccountRequest, new AccountCredentialMsg (inputName, inputPassword));
			break;
		case networkState.charselection:
			GUILayout.Box ("Charakterauswahl");
			int charsel = -1;
			charsel = GUILayout.SelectionGrid (charsel, avbCharNames, 1);
			if (charsel != -1) {
				ClientScene.AddPlayer (1);
				MyNetworkManager.client.Send (MyMsgType.HookChar, new StringMessage (avbCharIDs [charsel]));
			}
			if (GUILayout.Button ("Neuer Charakter")) {
				customCharPage = new CompactCreature ();
				customCharCreationStep = 0;
				currentState = networkState.createcharacter;
			}
			break;
		case networkState.createcharacter:
			showCustomCharCreation ();
			break;
		}
	}

	//Charcreation-GUI
	CompactCreature customCharPage;
	int customCharCreationStep = 0;
	Vector2 scrollHelper01 = new Vector2();
	int selectionRace01 = -1;

	void showCustomCharCreation ()
	{
		switch (customCharCreationStep) {
		case 0://Rasse auswählen
			GUILayout.Box("Volk");
			GUILayout.BeginHorizontal ();
			GUILayout.BeginScrollView (scrollHelper01, true, false,GUILayout.MinHeight(300));
			GUILayout.BeginVertical ();
			for (int i=0; i<MetaDataManager.instance.allPeoples.Count; i++) {
				if (GUILayout.Button (MetaDataManager.instance.allPeoples [i].Name))
					selectionRace01 = i;
			}
			GUILayout.EndVertical ();
			GUILayout.EndScrollView ();
			if (selectionRace01 != -1)
				GUILayout.Box (MetaDataManager.instance.allPeoples [selectionRace01].Description);
			GUILayout.EndHorizontal ();
			if (GUILayout.Button ("Weiter"))
				customCharCreationStep++;
			break;
		case 1://Attribute
			GUILayout.Box("Attribute");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Stärke:");
			customCharPage.bStrength=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bStrength.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Mut:");
			customCharPage.bCourage=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bCourage.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Gewandheit:");
			customCharPage.bAgility=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bAgility.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Fingerfertigkeit:");
			customCharPage.bPrestidigitation=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bPrestidigitation.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Konstitution:");
			customCharPage.bConstitution=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bConstitution.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Metabolismus:");
			customCharPage.bMetabolism=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bMetabolism.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Intelligenz:");
			customCharPage.bIntelligence=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bIntelligence.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Weisheit:");
			customCharPage.bWisdom=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bWisdom.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Schönheit:");
			customCharPage.bAppearance=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bAppearance.ToString()));
			GUILayout.EndHorizontal();
			GUILayout.BeginHorizontal();
			GUILayout.Label("Charisma:");
			customCharPage.bCharisma=System.Convert.ToInt16(GUILayout.TextField(customCharPage.bCharisma.ToString()));
			GUILayout.EndHorizontal();
			if(GUILayout.Button("Weiter"))
				customCharCreationStep++;
			if(GUILayout.Button("Zurück, alle Änderungen können verloren gehen!!!"))
				customCharCreationStep--;
			break;
		case 2:
			GUILayout.Box("Diverses");
			GUILayout.BeginHorizontal();
			GUILayout.Label("Name");
			customCharPage.name=GUILayout.TextField(customCharPage.name);
			GUILayout.EndHorizontal();
			if(GUILayout.Button("Weiter"))
				customCharCreationStep++;
			if(GUILayout.Button("Zurück"))
				customCharCreationStep--;
			break;
		case 3:
			if(GUILayout.Button("Charakter bestätigen und starten"))
				MyNetworkManager.client.Send(MyMsgType.RequestNewCharacterMessage,new ByteMessage(FileHelper.serializeObject<CompactCreature>(customCharPage)));
			break;

		
		
		
		}
			

	}
	

	//Referenz-Hilfefunktionen
	//Da nicht immer alle Objekte sichtbar sind, fragt der Watcher bei referenzAnfragen den Server ggf. um das Laden des entsprechenden Objekts zu Zugriffszwecken.


	public static T getReferenceObject<T> (string ID)where T:Object,IDInterface
	{
		ID = ID.Split ('-') [0];
		T result;
		List<T> objs = new List<T> (GameObject.FindObjectsOfType<T> ());
		if ((result = objs.Find (delegate(T obj) {
			return obj.getID () == ID;
		})) != default(IDObject)) 
			return result;
		else {
			MyNetworkManager.client.Send (MyMsgType.RequestVisibility, new StringMessage (ID));
			return getReferenceObject<T> (ID);//Falls dieser rekusive Aufruf das Programm blockiert, nullen und aufs beste hoffen...
		}
	}

	public static IRPGSource getReferenceSource (string ID)
	{
		IRPGSource result = getReferenceObject<RPGObject> (ID.Split ('-') [0]);
		if (ID.Contains ("-")) {
			result = (result as RPGObject).Effects.Find (delegate(TEffect obj) {
				return obj.getID () == ID;
			});
		}
		return result;
	}




}
