using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AddEffectMsg : MessageBase
{
	public byte[] effect;
	public string sourceID;
	public AddEffectMsg(){
	}
	public AddEffectMsg(byte[] e,string s){
		effect = e;
		sourceID = s;
	}
}

