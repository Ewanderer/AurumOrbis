using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;

public class AccountService : Behaviour
{

	public static AccountService instance;

	List<TAccount> registredAccount = new List<TAccount> ();

	public void Login(string name,string Password,GameObject player){
		TAccount acc = registredAccount.Find (delegate(TAccount obj) {
			return obj.humanName==name;
		});
		string[] charnames;
		string[] charids;
		int identity=acc.Login (Password,player.GetComponent<NetworkIdentity>().connectionToClient.connectionId,  out charnames,out charids);
		if (identity == -1) 
			NetworkServer.SendToClientOfPlayer (player, MyMsgType.SystemError, new StringMessage ("Fehler beim Einloggen. Nutzername oder Password falsch."));
		else
		NetworkServer.SendToClientOfPlayer (player,MyMsgType.LoginAnswer,new LoginAnswerMsg(identity,charnames,charids));
	}

	public void Register(string name, string Password,GameObject player){
		if (registredAccount.Exists (delegate(TAccount obj) {
			return obj.humanName == name;
		}))
			NetworkServer.SendToClientOfPlayer (player, MyMsgType.SystemError, new StringMessage ("Fehler bei der Accounterstellung! Nutzernamen bereits vergeben."));
		else{
			registredAccount.Add (new TAccount (name, Password));
			Login(name,Password,player);
		}
	}


	// Use this for initialization
	void Start ()
	{
		instance = this;
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

