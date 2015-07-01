using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Hier werden alle WorldBlocks und IDObjekte verwaltet
public class WorldGrid : MonoBehaviour
{
	public static WorldGrid instance;

	WorldBlock _main;//The highest WorldBlock, containing all other WorldBlocks.
	WorldBlock[,,] _baseGrid;//Das niedrigste Layer der Weltblöcke im 10x10x10m rahmen. [X,Z,Y]

	//Diese Funktion generiert entweder aus den Datein zu der ID ein IDObjekt oder sucht dieses aus der Liste der bereits geladenen IDObjekte
	public static IDObject getIDObject(string ID){
		return null;
	} 

	public WorldBlock[,,] baseGrid{
		get{return baseGrid;}
	}

	public WorldBlock main{
		get{return _main;}
	}




}

