using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Dieses Objekt ist für die Verwaltung aller Zeitabhänigen Dienste, sowie die generelle Initialiesierung aller Dienste verantwortlich
public class Core:MonoBehaviour {

	public static Core instance;

	void Start(){
		instance = this;
		InitializeServices ();
	} 

	void InitializeServices(){

	}



}
