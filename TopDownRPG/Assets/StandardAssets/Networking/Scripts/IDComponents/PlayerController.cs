using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerController : IDComponent
{
	public int boundAccountIdentiy;

	bool isHooked;
	bool isPlayer;

	//Diese Funktion überwacht das ordnungsgemäße Verbinden von Watcher. 


	//Diese Funktion wird vom Watcher aufgerufen, um die Kontrolle an den Spieler zu übergeben.
	public void HookIn(int identity){
		if (!isHooked && identity == boundAccountIdentiy)
			isPlayer = true;
	}


	public override void initialize ()
	{


		base.initialize ();
	}

	void Update(){
		if (isPlayer) {
		
		}
	}


}

