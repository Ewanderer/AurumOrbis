
using System.Collections;

public class RecieveDamageMsg
{
	public float _damage;
	public string _type;
	public string _id;

	public RecieveDamageMsg(float d,string t,string id){
		_damage = d;
		_type = t;
		_id = id;
	}
}

