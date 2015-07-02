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
	private Vector3 Position;

	private bool _isLoaded;//Solange die Blöcke nicht benötigt werden, wird ihr Speicher freigegeben und diese Variable wird auf false gesetzt.
	[SerializeField]
	private System.DateTime momentOfLastUpdate;//Um Arbeitsspeicher zu sparen, wird der Server nicht dauerhaft alle Blöcke berechnen, sondern diese bei Bedarf im Schnelldurchlauf laden
	[SerializeField]
	private Vector3[] _subBlocks;
	[SerializeField]
	private Vector3 _overBlock;
	[SerializeField]
	private List<string> _containedID = new List<string> ();//Liste aller enthaltenen Objekte anhand ihrer ID

	private List<IDObject> _loadedObjects=new List<IDObject>();

	public List<IDObject> loadedObjects{
		get{return _loadedObjects;}
	}

	public List<WorldBlock> subBlocks{
		get{
			return null;
		}
	} 

	public WorldBlock overBlock{
		get{
			return null;
		}
	}

	public bool isLoaded{
		get{return _isLoaded;}
	}

	//Diese Funktion wird aufgerufen, wenn das System Bedarf hat.
	public void loadUpBlock(){
		//Lese das Verzeichnis aus.
		//Lade die Objekte anhand ihrer ID's und erstelle sie(allerdings deaktivert)
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
		foreach (WorldBlock b in subBlocks) {
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
		string blockIdentityString = Layer.ToString () + "_" + ((int)Position.x).ToString () + "_" + ((int)Position.x).ToString () + "_" + ((int)Position.x).ToString () + ".wb";

	}

	//Diese Funktion speichert den Block und löscht anschließend alle im Chunk befindlichen Objekte, die nicht   
	public void closeBlock(){
		saveBlock();
		foreach (IDObject obj in loadedObjects)
			NetworkServer.Destroy (obj.gameObject);
		_isLoaded = false;
	}

}
