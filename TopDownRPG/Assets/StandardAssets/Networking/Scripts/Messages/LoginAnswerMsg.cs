using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class LoginAnswerMsg :MessageBase
{
	public int Accountidentity;
	public string[] characterNames;
	public string[] characterIDs;

	public LoginAnswerMsg(int identity,string[] characterNames,string[] characterIDs){
		Accountidentity = identity;
		this.characterNames = characterNames;
		this.characterIDs = characterIDs;
	}

	public LoginAnswerMsg(){
	}
}

