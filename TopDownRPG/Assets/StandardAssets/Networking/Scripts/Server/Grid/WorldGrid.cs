using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Hier werden alle WorldBlocks und IDObjekte verwaltet
public class WorldGrid : MonoBehaviour
{
	public static WorldGrid instance;

	WorldBlock _main;//The highest WorldBlock, containing all other WorldBlocks.
	WorldBlock[,,] _baseGrid;//Das niedrigste Layer der Weltblöcke im 10x10x10m rahmen. [X,Z,Y]

	List<PlayerController> allPlayerChars=new List<PlayerController>();

	//Basierend auf dem base-Grid werden die over-Blocks geschaffen, bis zu dem Punkt, dass es ein Layer mit nur einem Block gibt, dieser sei main.
	void createMetaGrid(){

	}

	//Diese Funktion generiert entweder aus den Datein zu der ID ein IDObjekt oder sucht dieses aus der Liste der bereits geladenen IDObjekte
	public static IDObject getIDObject(string ID){
		IDObject result;
		if ((result = WorldGrid.instance._main.loadedObjects.Find (delegate(IDObject obj) {
			return obj.id == ID;
		})) != default(IDObject))
			return result;
		else {
			//Objekt muss geladen werden.
		}
		return result;
	} 

	public WorldBlock[,,] baseGrid{
		get{return baseGrid;}
	}

	public WorldBlock main{
		get{return _main;}
	}

	//Diese Funktion lädt das gesamte Grid basierend auf einem im Ordner liegendem Grid...
	public void setupGridFromFile(){

	}





	public void EndPlayer(){
	
	}

	//Diese Funktion wird eine komplett leere Welt erschaffen(Wenn man wom naja offentsichtlichen)...
	public void setupEmptyGrid(Vector3 size){

	}



}

