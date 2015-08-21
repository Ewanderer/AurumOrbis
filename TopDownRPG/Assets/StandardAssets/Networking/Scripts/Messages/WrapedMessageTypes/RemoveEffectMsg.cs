
using System.Collections;

public class RemoveEffectMsg 
{
	public string effectID;
	public bool doenforce;

	public RemoveEffectMsg(string i,bool e){
		effectID = i;
		doenforce = e;
	}
}

