using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Hier werden alle WorldBlocks und IDObjekte verwaltet
public class WorldGrid : MonoBehaviour
{
	public static WorldGrid instance;

	public GridPoint pointExample;
	WorldBlock _main;//The highest WorldBlock, containing all other WorldBlocks.
	WorldBlock[,,] _baseGrid;//Das niedrigste Layer der Weltblöcke im 10x10x10m rahmen. [X,Y,Z]

	List<PlayerController> allPlayerChars=new List<PlayerController>();

	//Basierend auf dem base-Grid werden die over-Blocks geschaffen, bis zu dem Punkt, dass es ein Layer mit nur einem Block gibt, dieser sei main.
	void createMetaGrid(){
		//Berechne layerhöhe des BaseGrids
		int highestLayerX =Mathf.CeilToInt(Mathf.Log10( baseGrid.GetLength (0)));
		int highestLayerY =Mathf.CeilToInt(Mathf.Log10( baseGrid.GetLength (1)));
		int highestLayerZ =Mathf.CeilToInt(Mathf.Log10( baseGrid.GetLength (2)));
		int highestLayer;
		if (highestLayerX < highestLayerY)
		if (highestLayerY < highestLayerZ)
			highestLayer = highestLayerZ;
		else
			highestLayer = highestLayerY;
		else
			if (highestLayerX < highestLayerZ)
			highestLayer = highestLayerZ;
		else
			highestLayer = highestLayerX;
		_main = new WorldBlock (highestLayer,Vector3.zero);
		linkSubBlocks (_main, highestLayer);
		_main.saveBlock ();
	}

	void linkSubBlocks(WorldBlock overhead,int layer){
		if (layer > 0) {
			for (int x=0; x<10; x++)
				for (int y=0; y<10; y++)
					for (int z=0; z<10; z++) {
						WorldBlock nSub = new WorldBlock (layer - 1, new Vector3 (x, y, z));
						nSub.overBlock = overhead;
						linkSubBlocks (nSub, layer - 1);
						overhead.subBlocks.Add (new Vector3 (x, y, z));
						overhead.subBlocksRef.Add (nSub);
					}
		} else {
			for (int x=0; x<10; x++)
				for (int y=0; y<10; y++)
				for (int z=0; z<10; z++) {
					baseGrid[(int)overhead.position.x+x,(int)overhead.position.y+y,(int)overhead.position.z+z].overBlock=overhead;
				}
		}
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

	public static IRPGSource getSource(string ID){
		IRPGSource result;
		if (!ID.Contains ("-"))
			result = getIDObject (ID).GetComponent<RPGObject>();
		else {
			result=getIDObject(ID.Split('-')[0]).GetComponent<RPGObject>().Effects.Find(delegate(TEffect obj) {
				return obj.ownID.ToString()==ID.Split('-')[1];
			});
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
		//bestimmte höchste Layerebene

		//Lade den MainBlock
		_main = loadBlock (FileHelper.deserializeObject<int>(FileHelper.ReadFromFile("./server/blocks/identity.obj")), Vector3.zero);
	}

	WorldBlock loadBlock(int Layer,Vector3 Position){
		string blockfilestring = ("./server/blocks/"+StaticWorldManager.instance.usedWorld.WorldName+"/" + Layer.ToString () + "_" + ((int)Position.x).ToString () + "_" + ((int)Position.x).ToString () + "_" + ((int)Position.x).ToString () + ".bl");
		WorldBlock result = FileHelper.deserializeObject<WorldBlock> (FileHelper.ReadFromFile (blockfilestring));
		foreach (Vector3 v in result.subBlocks) {
			WorldBlock sub=loadBlock(Layer-1,v);
			sub.overBlock=result;
		}
		return result;
	}

	//Diese Funktion wird eine komplett leere Welt erschaffen(Wenn man wom naja offentsichtlichen)...
	public void setupEmptyGrid(Vector3 size){
		_baseGrid = new WorldBlock[(int)size.x, (int)size.y, (int)size.z];
		createMetaGrid ();
	}

	public void setupGridPoints(){
		for (int x=0; x<_baseGrid.GetLength(0); x++)
			for (int y=0; y<_baseGrid.GetLength(1); y++)
				for (int z=0; z<_baseGrid.GetLength(2); z++) {
				GridPoint nPoint=(GridPoint)Instantiate(pointExample,new Vector3(x,y,z),Quaternion.identity);
				nPoint.ConnectBlock(_baseGrid[x,y,z]);
				}
		NetworkServer.SpawnObjects ();
			}


}

