using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TWorld{
	public string WorldName;

	public struct TWorldObject{
		public Vector3 position;
		public Vector3 rotation;
		public Vector3 scale;
		public int assetID;
		public TWorldObject(Vector3 p,Vector3 r,Vector3 s,int id){
			position=p;
			rotation=r;
			scale=s;
			assetID=id;
		}
	}

	public class WorldNode{

		public List<TWorldObject> allObjects=new List<TWorldObject>();
		List<StaticObject> _loadedObjects =new List<StaticObject>();

		bool loading;

		public bool isLoaded{
			get{return allObjects.Count==_loadedObjects.Count;}
		}

		List<IDObject> _observers=new List<IDObject>();

		public List<StaticObject> loadedObjects{
			get{return _loadedObjects;}
		} 
		public List<IDObject> observers{
			get{return _observers;}
		}

		public void load(){
		if (loading)
				return;
			loading = true;
			int c=0;
			while (!isLoaded&&observers.Count>0) {
				GameObject g=(GameObject)Object.Instantiate(MyAssetDatabase.getAsset(allObjects[c].assetID));
				g.transform.position=allObjects[c].position;
				g.transform.rotation=Quaternion.Euler(allObjects[c].rotation);
				g.transform.localScale=allObjects[c].scale;
				c++;
			}
			loading = false;
			if (_observers.Count == 0)
				unload ();
		}

		public void unload(){
		if (loading)
				return;
			loading = true;
			foreach (StaticObject so in loadedObjects)
				GameObject.Destroy (so);
		}

	}
	public WorldNode[,,] Nodes;

	public TWorld(Vector3 size){
		Nodes=new WorldNode[(int)size.x,(int)size.y,(int)size.z];
	}
}

public class StaticWorldManager : MonoBehaviour
{


	public static StaticWorldManager instance;

	public static void registerIDObject(IDObject o, Vector3 node){
		instance.usedWorld.Nodes [(int)node.x, (int)node.y, (int)node.z].observers.Add (o);
		if (!instance.usedWorld.Nodes [(int)node.x, (int)node.y, (int)node.z].isLoaded)
			instance.usedWorld.Nodes [(int)node.x, (int)node.y, (int)node.z].load ();
	}

	public static void unregisterIDObject(IDObject o,Vector3 node){
		instance.usedWorld.Nodes [(int)node.x, (int)node.y, (int)node.z].observers.Remove (o);
		if(instance.usedWorld.Nodes [(int)node.x, (int)node.y, (int)node.z].observers.Count==0)
			instance.usedWorld.Nodes [(int)node.x, (int)node.y, (int)node.z].unload();
	}

	public TWorld usedWorld;

	public bool setUpWorld(string WorldName){
		usedWorld=FileHelper.deserializeObject<TWorld>(FileHelper.ReadFromFile("./data/worlds/"+WorldName+".wld"));
		return true;
	}

	public bool saveWorld(string Name){
		StaticObject[] foundObjects = GameObject.FindObjectsOfType<StaticObject> ();
		//Bestimme Weltgröße
		int minX=0;
		int minY=0;
		int maxX=0;
		int maxY=0;
		int minZ = 0;
		int maxZ = 0;
		foreach (StaticObject so in foundObjects) {
		if(so.getGridPosition().x<minX)
				minX=(int)so.getGridPosition().x;
			if(so.getGridPosition().x>maxX)
				maxX=(int)so.getGridPosition().x;
			if(so.getGridPosition().y<minY)
				minY=(int)so.getGridPosition().y;
			if(so.getGridPosition().y>maxY)
				maxY=(int)so.getGridPosition().y;
			if(so.getGridPosition().z<minZ)
				minZ=(int)so.getGridPosition().z;
			if(so.getGridPosition().z>maxZ)
				maxZ=(int)so.getGridPosition().z;
		}
		int sizeX = (maxX - minX)+1;
		int sizeY = (maxY - minY)+1;
		int sizeZ = (maxZ - minZ)+1;
		TWorld nWorld = new TWorld (new Vector3 (sizeX, sizeY, sizeZ));
		nWorld.WorldName = Name;
		foreach (StaticObject so in foundObjects) {
			Vector3 v=so.getGridPosition();
			nWorld.Nodes[(int)v.x,(int)v.y,(int)v.z].allObjects.Add(new TWorld.TWorldObject(so.transform.position,so.transform.rotation.eulerAngles,transform.localScale,so.assetID));
		}

		//Serialize to File
		FileHelper.WriteToFile("./data/worlds/"+Name+".wld",FileHelper.serializeObject<TWorld>(nWorld));
		return true;
	} 





}

