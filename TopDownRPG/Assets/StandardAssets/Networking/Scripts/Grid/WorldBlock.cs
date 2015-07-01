using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Diese Klasse dient zur Verwaltung der Daten von RPG-Objekten und VisualFX-Objekten, in jeweils fest defenierten Rahmen(testweise 10x10m).
//Dazu stellt diese Klasse alle notwendigen Funktionen zur Verfügung, um diese Blöcke in XML-Datein zu speichern und sich daraus zu generieren,
//sowie eine Instanzinierungsfunktion(mit evt. Netzwerkfunktionalität).

public class WorldBlock {

	struct WorldBlockHelper{
		int Layer;
		int X;
		int Y;
		int Z;
		System.DateTime momentOfLastUpdate;
		string[] objectIDs;
		Vector3[] _subBlocks;
		Vector3 _overBlock;
	}

	private int Layer;
	private bool _isLoaded;//Solange die Blöcke nicht benötigt werden, wird ihr Speicher freigegeben und diese Variable wird auf false gesetzt.
	private System.DateTime momentOfLastUpdate;//Um Arbeitsspeicher zu sparen, wird der Server nicht dauerhaft alle Blöcke berechnen, sondern diese bei Bedarf im Schnelldurchlauf laden
	private Vector3[] _subBlocks;
	private Vector3 _overBlock;

	private List<IDObject> containedObject = new List<IDObject> ();//Liste aller enthaltenen RPG-Objekte
	//private List<VisualFXObject> containedVFX=new List<VisualFXObject>();//Liste aller enthaltenen Visuelle Effekte

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
	}

	//Geht nach oben
	public void register(IDObject o){
		containedObject.Add (o);
		overBlock.register (o);
	}

	//Geht nach unten
	public void unregister(IDObject o){
		containedObject.Remove (o);
		foreach (WorldBlock b in subBlocks)
			b.unregister (o);
	}

	public void SaveBlock(string BlockIdentityString){

	}

}
