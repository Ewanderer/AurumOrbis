using UnityEngine;
using System.Collections;

public class RPGEvent {
	string _message;
	string _source;
	System.DateTime _timeCode;

	public string message{
		get{return _message;}
	}

	public string sourceObject {
		get{ return _source;}
	}

	public System.DateTime timeCode{
		get{return _timeCode;}
	}


	public string logMessage{
		get{
			return _source+":"+_message;
		}
	}

	public RPGEvent(string message, IRPGSource sourceObject){
		_message = message;
		_source = sourceObject.getID();
		_timeCode = System.DateTime.Now;

	}

	public RPGEvent(string message, string source){
		_message = message;
		_source = source;
		_timeCode = System.DateTime.Now;
	}

}
