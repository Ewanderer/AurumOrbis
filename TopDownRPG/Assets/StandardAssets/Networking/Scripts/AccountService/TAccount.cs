using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
[System.Serializable]
public class TAccount
{
	public static int idCounter;
	[SerializeField]
	int _identity;
	[SerializeField]
	string _humanName;
	[SerializeField]
	string _password;
	[SerializeField]
	List<string> _playerObjectsID=new List<string>();//ID of Player Character
	[SerializeField]
	List<string> playerObjectsName = new List<string> ();

	NetworkConnection connection;
	[SerializeField]
	bool _isAdmin;

	public bool isAdmin{
		get{return _isAdmin;}
	}

	public NetworkConnection usedConnection{
		get{return connection;}
	}

	//For creating new Account
	public TAccount(string name, string password){
		_humanName = name;
		_password = password;
		_identity = idCounter;
		idCounter++;
	}

	public string humanName{
		get{return _humanName;}
	}

	public int Login(string password, NetworkConnection conn ,out string[] Names,out string[] IDs){
		if (_password == password&&connection==null) {
			Names=null;
			IDs=_playerObjectsID.ToArray();
			connection=conn;
			return _identity;
		}
		Names = null;
		IDs = null;
			return -1;
	}

	public void Logout(){
		connection = null;
	}

	public void addCharacter(IDObject character){
		_playerObjectsID.Add (character.id);
		playerObjectsName.Add (character.GetComponent<TCreature> ().name);
	}



}

