using UnityEngine;
using System.Collections;

public struct Content
{
	public string Information;
	public string[] StrongRequirements;//Alle Bediengungen hier müssen erfüllt sein
	public string[] Requirements;//Es müssen nur soviele Bediengungen wie in NONR angeben sind erfüllt sein
	public int NumberofNeededRequirments;//NONR

	public Content(string _information,string[] _strongRequirements, string[] _requirements,int _numberOfNeededRequirments){
		Information = _information;
		StrongRequirements = _strongRequirements;
		Requirements = _requirements;
		NumberofNeededRequirments = _numberOfNeededRequirments;
	}

	//Für eine dynamische Auslesung aus Files
	public Content(string filestring){
		string[] blocks=filestring.Split((char)161);
		Information = blocks [0];
		StrongRequirements=blocks[1].Split(';');
		Requirements=blocks[2].Split(';');
		NumberofNeededRequirments = System.Convert.ToInt16 (blocks [3]);
	}

}

