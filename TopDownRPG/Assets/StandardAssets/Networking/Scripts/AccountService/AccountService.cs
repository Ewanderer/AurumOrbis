using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using System.Collections;
using System.Collections.Generic;

public class AccountService : MonoBehaviour
{

	public static AccountService instance;

	public List<TAccount> registredAccount = new List<TAccount> ();

	public void Login(NetworkMessage msg){

		AccountCredentialMsg credential = msg.ReadMessage<AccountCredentialMsg> ();
		TAccount acc = registredAccount.Find (delegate(TAccount obj) {
			return obj.humanName==credential.userName;
		});
		if (acc == default(TAccount)) {
			NetworkServer.SendToClient (msg.conn.connectionId, MyMsgType.SystemError, new StringMessage ("Fehler beim Einloggen. Nutzername oder Password falsch."));
			return;
		}
			string[] charnames;
		string[] charids;

		int identity=acc.Login (credential.passsword,msg.conn,  out charnames,out charids);
		if (identity == -1) 
			NetworkServer.SendToClient(msg.conn.connectionId, MyMsgType.SystemError, new StringMessage ("Fehler beim Einloggen. Nutzername oder Password falsch."));
		else
		NetworkServer.SendToClient (msg.conn.connectionId,MyMsgType.LoginAnswer,new LoginAnswerMsg(identity,charnames,charids));
	}

	public void Register(NetworkMessage msg){

		AccountCredentialMsg credential = msg.ReadMessage<AccountCredentialMsg> ();
		if (registredAccount.Exists (delegate(TAccount obj) {
			return obj.humanName == credential.userName;
		}))
			NetworkServer.SendToClient (msg.conn.connectionId, MyMsgType.SystemError, new StringMessage ("Fehler bei der Accounterstellung! Nutzernamen bereits vergeben."));
		else{
			registredAccount.Add (new TAccount (credential.userName, credential.passsword));
			Login(msg);
		}
	}




	// Use this for initialization
	void Start ()
	{
		instance = this;
		NetworkServer.RegisterHandler (MyMsgType.LoginRequest, Login);
		NetworkServer.RegisterHandler (MyMsgType.NewAccountRequest, Register);
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void Logout(NetworkConnection conn){
		registredAccount.Find (delegate(TAccount obj) {
			return obj.usedConnection == conn;
		}).Logout ();
	}

	public void AddCharacter(NetworkConnection conn, IDObject playerObject){
		registredAccount.Find (delegate(TAccount obj) {
			return obj.usedConnection == conn;
		}).addCharacter (playerObject);
	}
}

