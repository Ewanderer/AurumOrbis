using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Dieses Objekt ist für die Verwaltung aller Zeitabhänigen Dienste, sowie die generelle Initialiesierung aller Dienste verantwortlich, hier sind auch alle für Console oder Skripte gebundene Kommandos aufgeführt.
public class RPGCore:MonoBehaviour {
	public static RPGCore instance;

	public static EffectCatalog effectCatalog;

	void Start(){
		instance = this;
		InitializeServices ();
	} 

	void InitializeServices(){
		Dice.InitializeDiceService();
		effectCatalog = new EffectCatalog ("./");
	}



	//System-Befehle(RPG-Intern):

	//Diese Funktion lädt aus der Datenbank die EffectBase mit diesem Namen, generiert anschließend den Effekt
	public TEffect spawnEffect(string EffectName,IRPGSource Source,RPGObject Target){
		return new TEffect(Target.getID());
	}

}
