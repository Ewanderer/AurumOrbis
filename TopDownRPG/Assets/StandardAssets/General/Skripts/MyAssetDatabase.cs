using UnityEngine;
using System.Collections;

public class MyAssetDatabase:MonoBehaviour {
	[System.Serializable]
	public struct Asset{
		public string _name;
		public Object _object;
	}

	public Asset[] allAssets;

	static MyAssetDatabase instance;

	public static Object getAsset(int index){
		return instance.allAssets [index]._object;
	}
	void Start(){
		instance = this;
	}

}
