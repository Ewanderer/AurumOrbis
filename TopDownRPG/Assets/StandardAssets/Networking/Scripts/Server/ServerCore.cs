using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;

public class ServerCore : MonoBehaviour {

	public void createObjectFromID(string ID){
		
	}

	public void HookPlayerUp(NetworkMessage msg){
		//Load Player
		IDObject playerchar = (IDObject)Instantiate (MyNetworkManager.instance.baseObject);
		playerchar.loadFromFile (msg.ReadMessage<StringMessage>().value);
		NetworkServer.AddPlayerForConnection (msg.conn, playerchar.gameObject, 1);
	}

	void Start(){
		NetworkServer.RegisterHandler (MyMsgType.HookChar, HookPlayerUp);
	}

}
