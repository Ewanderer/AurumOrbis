using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Dieses Objekt ist für die Verwaltung aller Zeitabhänigen Dienste, sowie die generelle Initialiesierung aller Dienste verantwortlich, hier sind auch alle für Console oder Skripte gebundene Kommandos aufgeführt.
public class Core:MonoBehaviour {

	public static Core instance;

	public static EffectCatalog effectCatalog;

	void Start(){
		instance = this;
		InitializeServices ();
	} 

	void InitializeServices(){
		Dice.InitializeDiceService();
		effectCatalog = new EffectCatalog ("./");
	}



	//System-Befehle

	//Dieses Kommando lädt aus der AssetBibliothek basierend auf der prefabID das Objekt und initialisiert es in der Welt.
	public void SpawnObject(string prefabID,Vector3 position,Quaternion rotation){

	}

	//Diese Funktion ist eine Erweiterte Form von Spawn-Objekt, die allerdings auf die Notwendigkeiten bei NPC abgestimmt ist. identityString ist hierbei nur Platzhalter für eine evt. später implementierte NPC-Behaviour Struktur/Klasse
	public void SpawnNPC(string prefabID,Vector3 position,Quaternion rotation,string identityString){

	}

	//Diese Funktion lädt aus der Datenbank die EffectBase mit diesem Namen, generiert anschließend den Effekt
	public TEffect SpawnEffect(string EffectName,IRPGSource Source,RPGObject Target){
		return new TEffect();
	}

}
