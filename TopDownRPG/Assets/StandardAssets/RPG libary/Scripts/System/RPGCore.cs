using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Dieses Objekt ist für die Verwaltung aller Zeitabhänigen Dienste, sowie die generelle Initialiesierung aller Dienste verantwortlich, hier sind auch alle für Console oder Skripte gebundene Kommandos aufgeführt.
public class RPGCore:MonoBehaviour {

	public IDObject baseObject;

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



	//System-Befehle:

	//Diese Funktion generiert ein neues IDObjekt, lädt die Objekt-Daten und schickt anschließend den Spawn an die Clienten.
	public void spawnObjectFromData(string id){
		IDObject result = new IDObject ();
		//If not existing
		result = Instantiate (baseObject) as IDObject;
		//Fill result with IDInformation from File...
		result.GetComponent<NetworkIdentity> ().ForceSceneId (result.id);

		//else just set result with found
		return result;
	}

	//Diese Funktion lädt aus der Datenbank die EffectBase mit diesem Namen, generiert anschließend den Effekt
	public TEffect spawnEffect(string EffectName,IRPGSource Source,RPGObject Target){
		return new TEffect();
	}

}
