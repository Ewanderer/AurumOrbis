using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Diese Klasse stellt allen ID-Objekten einzigartige IDs zur Verfügung.
public class IDService : MonoBehaviour {


	private ulong newID=1;//Wenn das nicht ausreicht, ist die Welt defenitiv zu groß...
	private List<string> freeIDs=new List<string>();

	private static IDService instance;

	void Start(){
		if(IDService.instance==null){
			IDService.instance=this;
		}
	}

	public static string getUniqueID(){
		string result;
		if (instance.freeIDs.Count > 0) {
			result = instance.freeIDs [0];
			instance.freeIDs.RemoveAt (0);
		} else {
			result=instance.newID.ToString();
			instance.newID++;
		}
		return result;
	}

	public static void freeID(string id){
		instance.freeIDs.Add (id);
	}


}
