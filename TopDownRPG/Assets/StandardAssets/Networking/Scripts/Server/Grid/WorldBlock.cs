using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

//Diese Klasse dient zur Verwaltung der Daten von RPG-Objekten und VisualFX-Objekten, in jeweils fest defenierten Rahmen(testweise 10x10m).
//Dazu stellt diese Klasse alle notwendigen Funktionen zur Verfügung, um diese Blöcke in XML-Datein zu speichern und sich daraus zu generieren,
//sowie eine Instanzinierungsfunktion(mit evt. Netzwerkfunktionalität).

public class WorldBlock {
	[SerializeField]
	private int Layer;
	[SerializeField]
	private Vector3 _position;

	private bool _isLoaded=false;//Solange die Blöcke nicht benötigt werden, wird ihr Speicher freigegeben und diese Variable wird auf false gesetzt.
	[SerializeField]
	private System.DateTime momentOfLastUpdate;//Um Arbeitsspeicher zu sparen, wird der Server nicht dauerhaft alle Blöcke berechnen, sondern diese bei Bedarf im Schnelldurchlauf laden
	[SerializeField]
	private List<Vector3> _subBlocks;
	[SerializeField]
	private Vector3 _overBlock;
	[SerializeField]
	private List<string> _containedID = new List<string> ();//Liste aller enthaltenen Objekte anhand ihrer ID

	private List<WorldBlock> _subBlocksRef = new List<WorldBlock> ();
	private WorldBlock _overBlockRef=null;

	private List<IDObject> _loadedObjects=new List<IDObject>();

	public List<IDObject> loadedObjects{
		get{return _loadedObjects;}
	}

	public List<Vector3> subBlocks{
		get{return _subBlocks;}
	}


	public List<WorldBlock> subBlocksRef{
		get{
			return _subBlocksRef;
		}
	} 

	public WorldBlock overBlock{
		get{
			return _overBlockRef;
		}
		set{
			_overBlockRef=value;
		}
	}

	public bool isLoaded{
		get{return _isLoaded;}
	}

	public Vector3 position{
		get{return _position;}
	}

	//Diese Funktion wird aufgerufen, wenn das System Bedarf hat.
	public void openBlock(){
	


		//Lade die Objekte anhand ihrer ID's und erstelle sie(allerdings deaktivert)
		System.TimeSpan difference = System.DateTime.UtcNow - momentOfLastUpdate;
		float timedifference=(float)difference.TotalSeconds;
		//Führe bei allen Objekten das Überbrückungs-Update anhand der momentOfLastUpdate ran,sofern diese beim Laden geupdatet werden möchten.

		//Alternative Langzeit-UpdateRoutine auf Basis der Statistiken. Wird später implementiert.

		//Aktivierung aller Objekte, sodass die normalen UpdateRoutinen greifen.
		_isLoaded = true;
	}

	//Geht nach oben
	public void register(IDObject o){
		_containedID.Add (o.id);
		loadedObjects.Add (o);
		overBlock.register (o);
	}

	//Geht nach unten
	public void unregister(IDObject o){
		_containedID.Remove (o.id);
		loadedObjects.Remove(o);
		foreach (WorldBlock b in subBlocksRef) {
			b.unregister (o);
		}
	}

	public void register(string id){
		_containedID.Add (id);
		if (isLoaded) {
			//Das Objekt muss noch generiert werden

		} else {
			overBlock.register(id);
		}
	}

	public void saveBlock(){
		string blockfilestring = ("./server/blocks/"+StaticWorldManager.instance.usedWorld.WorldName+"/" + Layer.ToString () + "_" + ((int)_position.x).ToString () + "_" + ((int)_position.x).ToString () + "_" + ((int)_position.x).ToString () + ".bl");
		FileHelper.WriteToFile (blockfilestring, FileHelper.serializeObject<WorldBlock> (this));
		foreach (WorldBlock b in _subBlocksRef)
			b.saveBlock ();
	}

	//Diese Funktion speichert den Block und löscht anschließend alle im Chunk befindlichen Objekte, die nicht   
	public void closeBlock(){
		momentOfLastUpdate = System.DateTime.UtcNow;
		saveBlock();
		foreach (IDObject obj in loadedObjects)
			NetworkServer.Destroy (obj.gameObject);
		_isLoaded = false;
	}

	public WorldBlock(int l,Vector3 v){
		Layer = l;
		_position = v;
	}

}
