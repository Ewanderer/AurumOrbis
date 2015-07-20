using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class MyNetworkManager : MonoBehaviour
{
	public static MyNetworkManager instance;
	public static NetworkClient client;
	static bool IsNetworkUp;
	public Watcher playerObject;
	public IDObject baseObject;
	public GameObject Core;


	// Use this for initialization
	void Start ()
	{
		IsNetworkUp = true;

	}
	
	// Update is called once per frame
	void Update ()
	{

	}

	string port="4444";
	string adress="127.0.0.1";

	void OnGUI(){
		if (IsNetworkUp) {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Adresse:");
			adress=GUILayout.TextField(adress,GUILayout.MinWidth(120));
			GUILayout.Label("Port:");
			port=GUILayout.TextField(port,GUILayout.MinWidth(50));
			GUILayout.EndHorizontal();
			if(GUILayout.Button("Server+Client")){
				SetupServer ();
				SetupLocalClient ();
			}
			if(GUILayout.Button("Server")){
				SetupServer();
			}
			if(GUILayout.Button("Client")){
				SetupClient();
			}
		}
	}

	void SetupServer(){
	
		Debug.Log("Server wird aufgesetzt...");
		NetworkServer.Listen (System.Convert.ToInt16(port));
		Instantiate (Core);
		NetworkServer.SpawnObjects ();
		IsNetworkUp = false;
		Debug.Log("Server bereit.");
	}

	void SetupClient(){
		Debug.Log("Verbinde mit Server...");
		client = new NetworkClient ();
		client.RegisterHandler (MsgType.Connect, OnConnected);
		client.Connect (adress, System.Convert.ToInt16 (port));
	}

	void SetupLocalClient(){
		Debug.Log("Lokaler Client wird aufgesetzt...");
		client = ClientScene.ConnectLocalServer ();
		client.RegisterHandler (MsgType.Connect, OnConnected);
	}

	void OnConnected(NetworkMessage netMsg){
		Debug.Log("Verbindung mit Server hergestellt.");
		Instantiate (playerObject);
		IsNetworkUp = false;
	}





}

