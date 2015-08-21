
using System.Collections;

public class AddEffectMsg 
{
	public TEffect effect;
	public string sourceID;

	public AddEffectMsg(TEffect e,string s){
		effect = e;
		sourceID = s;
	}
}

