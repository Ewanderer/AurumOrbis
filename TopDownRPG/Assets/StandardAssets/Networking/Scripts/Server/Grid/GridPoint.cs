using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GridPoint : NetworkBehaviour{
	[SyncVar]
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
