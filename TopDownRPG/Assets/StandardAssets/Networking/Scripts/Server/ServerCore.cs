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
		NetworkServer.Spawn (obj.gameObject);
		obj.loadFromFile (ID);
		obj.distributeObjectStateToObservers ();
	}

	public void HookPlayerUp(NetworkMessage msg){
		//Load Player
		IDObject playerchar = (IDObject)Instantiate (MyNetworkManager.instance.baseObject);
		NetworkServer.AddPlayerForConnection (msg.conn, playerchar.gameObject, 1);
		playerchar.loadFromFile (msg.ReadMessage<StringMessage>().value);
		playerchar.distributeObjectStateToObservers ();
	}

	public void onCreateObjectFromTemplateMsg(NetworkMessage msg){
		
	}


	public void createObjectFromTemplate(string templateID,Vector3 position,Vector3 rotation){
	
	}
	//Und hier mit physikparameter
	public void createObjectFromTemplate(string templateID,Vector3 position,Vector3 rotation, Vector3 velocity,Vector3 angularVelocity){
	
	}

	public void onCreatePlayerObjectRequestMsg(NetworkMessage msg){
		createPlayerObject (FileHelper.deserializeObject<CompactCreature> (msg.ReadMessage<ByteMessage> ().value), msg.conn);
	}

	public void createPlayerObject(CompactCreature charpage,NetworkConnection conn){
		IDObject pb= Instantiate<IDObject> (MyNetworkManager.instance.baseObject);
		pb.Cmd_registerObject ();
		TCreature c= pb.gameObject.AddComponent<TCreature> ();
		c.setupObjectByCompact (charpage as CompactCreature, true);
		pb.gameObject.AddComponent<CharacterController> ();
		AccountService.instance.AddCharacter (conn, pb);
		NetworkServer.AddPlayerForConnection (conn, pb.gameObject, 0);
	}



	void Start(){
		NetworkServer.RegisterHandler (MyMsgType.HookChar, HookPlayerUp);
		NetworkServer.RegisterHandler (MyMsgType.CreateObjectFromID, OnCreateObjectFromIDMsg);
		NetworkServer.RegisterHandler (MyMsgType.CreateObjectFromTemplate, onCreateObjectFromTemplateMsg);
		NetworkServer.RegisterHandler (MyMsgType.RequestNewCharacterMessage, onCreatePlayerObjectRequestMsg);
		//NetworkServer.RegisterHandler(MsgType.)
	}

}
