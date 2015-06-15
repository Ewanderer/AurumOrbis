using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class EffectCatalog
{

	private List<TEffectBase> catalog = new List<TEffectBase> ();

	public TEffectBase getEffectBase (string Name){
		return catalog.Find (delegate(TEffectBase obj) {
			return obj.Name == Name;
		});
	}

	public EffectCatalog (string FileName)
	{

		//Erstelle eine Liste aller EffectBase
		string[] effectBaseFileNames = Directory.GetFiles (FileName, "*.ebf", SearchOption.AllDirectories);
		foreach (string path in effectBaseFileNames) {
			catalog.Add (new TEffectBase (System.IO.File.ReadAllText (path)));
		}
		for (int x=0; x<catalog.Count; x++)
			for (int y=x+1; y<catalog.Count; y++) {
			if(catalog[x].Name==catalog[y].Name)
				Debug.LogError("Der Effekt mit dem Namen "+catalog[x].Name+" exestiert doppelt.");
		}
		
	}


}

