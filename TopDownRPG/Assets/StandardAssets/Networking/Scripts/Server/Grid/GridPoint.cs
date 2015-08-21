using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Wenn dieses Objekt eingesehen werden kann, muss der angehängte Weltblock geladen werden. Der Vorteil ist, dass nur echte Spieler dieses System ausladen, sodass einfache NPC einfach einfrieren mit dem Rest ihres Blocks.
public class GridPoint : NetworkBehaviour{
	private WorldBlock connectedBlock=null;
	public void ConnectBlock(WorldBlock wb){
		if(connectedBlock==null)
		connectedBlock = wb;
	}

	void Update(){
	if (!isLocalPlayer)
			return;
		if (gameObject.GetComponent<NetworkIdentity> ().observers.Count > 0 && !connectedBlock.isLoaded)
			connectedBlock.openBlock ();
		if (gameObject.GetComponent<NetworkIdentity> ().observers.Count == 0 && connectedBlock.isLoaded)
			connectedBlock.closeBlock ();
	}
}
