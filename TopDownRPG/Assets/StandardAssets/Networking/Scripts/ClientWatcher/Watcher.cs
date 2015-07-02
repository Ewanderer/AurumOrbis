using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Dieser Skript verwaltet das Laden von Spielfigur, sowie der Erstellen der Statischen Umgebung dieser und dem Abrufen der zugehörigen IDObjekte
public class Watcher : NetworkBehaviour {



	public static Watcher instance;

	//Da nicht immer alle Objekte sichtbar sind, fragt der Watcher bei referenzAnfragen den Server ggf. um das Laden des entsprechenden Objekts zu Zugriffszwecken.
	public IDObject getIDObject(string ID){
		return null;
	}

	// Use this for initialization
	void Start () {
	
	}

	public override void OnStartLocalPlayer ()
	{
		base.OnStartLocalPlayer ();
		foreach(Camera c in Camera.allCameras)
			c.tag="";
		gameObject.tag="Main Camera";
		if (base.isClient)
			instance = this;
	}

	public void checkGridPoints(){
		foreach (GridPoint gp in GameObject.FindObjectsOfType<GridPoint>()) {
			//Logge bei allen "sichtbaren" GridPoints ein
			//Logge bei allen "nicht-sichtbaren" GridPoints aus
		}
	}



}
