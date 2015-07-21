using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class RecieveDamageMsg:MessageBase
{
	public float _damage;
	public string _type;
	public string _id;
	public RecieveDamageMsg(){
	}
	public RecieveDamageMsg(float d,string t,string id){
		_damage = d;
		_type = t;
		_id = id;
	}
}

