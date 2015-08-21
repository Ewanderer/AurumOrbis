using UnityEngine;
using System.Collections;
//Diese Klasse verwaltet die Übernahme und Ausführung selbiger, stellt ggf. auch den Initative-Balken der GUI bereit.
[RequireComponent(typeof(TCreature))]
public class ActionManager : IDComponent
{
	public bool CombatMode;//Außerhalb des Combat-Mode wird das Invoking einer Aktion bis zum nächsten Freien Wechsel verzögert. So kann man in Ruhe Menüaufwenige Aktionen ausführen, wie Craften
	Executable currentAction;

	public const float _roundDuration=1.5f;//Dieser Wert gibt an, wie lange eine Runde dauert.

	//Diese Hilfswerte geben an, wie lang das Zeitfenster ist, in dem man frei seine Aktionen wechsel kann.
	private float frameOne{
		get{
			return Mathf.Clamp(_roundDuration/3+this.GetComponent<TCreature>().cInitative/200,_roundDuration/3,_roundDuration*0.75f);
		}
	}

	private float frameTwo{
		get{
			return Mathf.Clamp((_roundDuration-frameOne)/2+this.GetComponent<TCreature>().cInitative/100,frameOne,_roundDuration);
		}
	}

	private float frameThree{
		get{
			return Mathf.Clamp(_roundDuration-frameTwo,0,_roundDuration);
		}
	}

	public override void deserializeFromFile (string FileName)
	{

	}

	public override void initialize ()
	{
		base.initialize ();
	}

	public override void OnNetworkUpdate (UnityEngine.Networking.NetworkMessage msg)
	{

	}

	public override void serializeToFile (string FileName)
	{

	}

	public override void update (float timeSinceLastUpdate)
	{
		base.update (timeSinceLastUpdate);
	}

	//Diese Funktion übernimmt eine neue Aktion
	public void setAction(){

	}

	public override IDComponentUpdateMsg CreateInitialSetupMessage ()
	{
		return null;
	}

}

