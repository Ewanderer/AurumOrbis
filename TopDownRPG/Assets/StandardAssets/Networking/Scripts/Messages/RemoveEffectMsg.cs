using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RemoveEffectMsg : MessageBase
{
	public string effectID;
	public bool doenforce;
	public RemoveEffectMsg(){
	}
	public RemoveEffectMsg(string i,bool e){
		effectID = i;
		doenforce = e;
	}
}

