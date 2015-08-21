using UnityEngine;
using UnityEngine.Networking;
using System.Collections;


public class ByteMessage : MessageBase
{
	public byte[] value;
	public ByteMessage(){
	}
	public ByteMessage(byte[] bytes){
		value = bytes;
	}
}

