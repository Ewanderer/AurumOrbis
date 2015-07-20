using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : IDComponent
{
	public int boundAccountIdentiy;
	//Diese Funktion überwacht das ordnungsgemäße Verbinden von Watcher. 


	//Diese Funktion wird vom Watcher aufgerufen, um die Kontrolle an den Spieler zu übergeben.
	public void HookIn(int identity){

	}


	public override void initialize ()
	{


		base.initialize ();
	}

	public override void update (float timeSinceLastUpdate)
	{
		//Hier das update einberufen
		base.update (timeSinceLastUpdate);
	}


}

