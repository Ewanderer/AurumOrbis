using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class GridPoint : NetworkBehaviour{

	private WorldBlock connectedBlock;
	private List<Watcher> loggedClients;

	[Command]
	public void Cmd_OnClientEnter(Watcher w){
		loggedClients.Remove (w);
		loggedClients.Add (w);
		if (!connectedBlock.isLoaded) {
			connectedBlock.loadUpBlock();
		}
	}

	[Command]
	public void Cmd_OnClientLeave(Watcher w){
		loggedClients.Remove (w);
		if (loggedClients.Count == 0) {
			connectedBlock.closeBlock();
		}
	}

}
