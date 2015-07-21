using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Dieses Objekt ist für die Verwaltung aller Zeitabhänigen Dienste, sowie die generelle Initialiesierung aller Dienste verantwortlich, hier sind auch alle für Console oder Skripte gebundene Kommandos aufgeführt.
public class RPGCore:MonoBehaviour {
	public static RPGCore instance;
	

	void Start(){
		instance = this;
		InitializeServices ();
	} 

	void InitializeServices(){
		Dice.InitializeDiceService();
	}



	//System-Befehle(RPG-Intern):

	//Diese Funktion lädt aus der Datenbank die EffectBase mit diesem Namen, generiert anschließend den Effekt
	public TEffect spawnEffect(string EffectName){
		 TEffect e=MetaDataManager.instance.allEffects.Find (delegate(TEffect obj) {
			return obj.Name == EffectName;
		});
		return new TEffect (e.Name, e.GeneralCategory, e.Tags, e.GeneralOrder, e.PassiveEffectStrings, e.ActiveEffectStrings, e.oDuration);

	}

}
