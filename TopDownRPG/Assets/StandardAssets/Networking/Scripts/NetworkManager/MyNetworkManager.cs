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
	public GridPoint pointObject;
	public GameObject Core;

	public bool mode;//true=Server,false=Real-Client

	// Use this for initialization
	void Start ()
	{
		IsNetworkUp = true;
		instance = this;
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
		mode = true;
		Debug.Log("Server wird aufgesetzt...");
		NetworkServer.Listen (System.Convert.ToInt16(port));
		Instantiate (Core);
		NetworkServer.SpawnObjects ();
		IsNetworkUp = false;
		Debug.Log("Server bereit.");
		NetworkServer.RegisterHandler (MyMsgType.IDComponentUpdateMessage, OnIDComponentUpdateMsg);
	}

	void SetupClient(){
		ClientScene.RegisterPrefab (this.pointObject.gameObject);
		ClientScene.RegisterPrefab (this.baseObject.gameObject);
		mode = false;
		Debug.Log("Verbinde mit Server...");
		client = new NetworkClient ();
		client.RegisterHandler (MsgType.Connect, OnConnected);
		client.Connect (adress, System.Convert.ToInt16 (port));
		client.RegisterHandler (MyMsgType.IDComponentUpdateMessage, OnUpdateDistributionRequestMsg);
		ClientScene.AddPlayer (0);
	}

	void SetupLocalClient(){
		Debug.Log("Lokaler Client wird aufgesetzt...");
		client = ClientScene.ConnectLocalServer ();
		client.RegisterHandler (MsgType.Connect, OnConnected);
		ClientScene.AddPlayer (0);
	}

	void OnConnected(NetworkMessage netMsg){
		Debug.Log("Verbindung mit Server hergestellt.");
		Instantiate (playerObject);
		IsNetworkUp = false;
	}


	public static void DistributeIDComponentUpdate(IDComponent sender,IDComponentUpdateMsg msg){
		msg.id = sender.getID();
		msg.componentName = sender.GetType ().ToString ();
		if (instance.mode) 
			NetworkServer.SendToAll (MyMsgType.IDComponentUpdateMessage, msg);
		else {
			client.Send(MyMsgType.IDComponentUpdateMessage,msg);
		}
	}
	//Setzt das Update auf Client/Server um
	public void OnIDComponentUpdateMsg(NetworkMessage msg){
		IDComponentUpdateMsg M = msg.ReadMessage<IDComponentUpdateMsg> ();
		foreach (IDObject o in GameObject.FindObjectsOfType<IDObject>())
			if (o.id == M.id) {
			if(o.GetComponent(M.componentName)==null)
				o.gameObject.AddComponent(Types.GetType(M.componentName,""));
			(o.GetComponent(M.componentName)as IDComponent).OnNetworkUpdate(msg);
			return;
		}
	}
	//Update wird vom Server an jeden, außer den Absender verschickt.
	public void OnUpdateDistributionRequestMsg(NetworkMessage msg){
		foreach (NetworkConnection c in NetworkServer.connections)
			if (c != msg.conn)
				NetworkServer.SendToClient (c.connectionId, MyMsgType.IDComponentUpdateMessage, msg.ReadMessage<IDComponentUpdateMsg>());
	}




}

