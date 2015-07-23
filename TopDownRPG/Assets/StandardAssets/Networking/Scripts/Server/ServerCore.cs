using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class ServerCore : MonoBehaviour {

	public void OnCreateObjectFromIDMsg(NetworkMessage msg){
		createObjectFromID (msg.ReadMessage<StringMessage> ().value);
	}

	public void createObjectFromID(string ID){
		IDObject obj = Instantiate<IDObject> (MyNetworkManager.instance.baseObject);
		obj.loadFromFile (ID);
		NetworkServer.SpawnObjects ();
	}

	public void HookPlayerUp(NetworkMessage msg){
		//Load Player
		IDObject playerchar = (IDObject)Instantiate (MyNetworkManager.instance.baseObject);
		playerchar.loadFromFile (msg.ReadMessage<StringMessage>().value);
		NetworkServer.AddPlayerForConnection (msg.conn, playerchar.gameObject, 1);
	}

	public void onCreateObjectFromTemplateMsg(NetworkMessage msg){
		
	}

	public void createObjectFromTemplate(string templateID,Vector3 position,Vector3 rotation){
	
	}
	//Und hier mit physikparameter
	public void createObjectFromTemplate(string templateID,Vector3 position,Vector3 rotation, Vector3 velocity,Vector3 angularVelocity){
	
	}

	void Start(){
		NetworkServer.RegisterHandler (MyMsgType.HookChar, HookPlayerUp);
		NetworkServer.RegisterHandler (MyMsgType.CreateObjectFromID, OnCreateObjectFromIDMsg);
		NetworkServer.RegisterHandler (MyMsgType.CreateObjectFromTemplate, onCreateObjectFromTemplateMsg);
	}

}
