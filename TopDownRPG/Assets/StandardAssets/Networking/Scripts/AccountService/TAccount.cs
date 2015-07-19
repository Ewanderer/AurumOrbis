using UnityEngine;
using System.Collections;
using System.Collections.Generic;

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
	int connectionID=-1;
	[SerializeField]
	bool _isAdmin;

	public bool isAdmin{
		get{return _isAdmin;}
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

	public int Login(string password, int connection,out string[] Names,out string[] IDs){
		if (_password == password) {
			Names=null;
			IDs=_playerObjectsID.ToArray();
			return _identity;
		}
		Names = null;
		IDs = null;
			return -1;
	}



}

