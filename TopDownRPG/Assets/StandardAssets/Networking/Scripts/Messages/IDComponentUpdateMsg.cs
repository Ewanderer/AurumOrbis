using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class IDComponentUpdateMsg :MessageBase 
{
	public string id;
	public string componentName;

	public short updateType;//0 ist immer eine komplette Überarbeitung
	public byte[] data;
	public IDComponentUpdateMsg(short t,byte[] d){
	
		updateType = t;
		data = d;

	}
	public IDComponentUpdateMsg(){

	}
}

