using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class AccountCredentialMsg : MessageBase
{
	public string userName;
	public string passsword;
	public AccountCredentialMsg(string n,string p){
		userName = n;
		passsword = p;
	}
	public AccountCredentialMsg(){}
}

