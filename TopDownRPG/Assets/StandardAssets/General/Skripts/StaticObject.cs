using UnityEngine;
using System.Collections;

//Dieses Lustige Objekt dient zur Kennzeichnung und Sortierung von Statischen Elementen in die Speicher der StaticWorld...
public class StaticObject : MonoBehaviour {

	public int assetID;

	public Vector3 getGridPosition(){
		return new Vector3 (Mathf.FloorToInt (transform.position.x / 10), Mathf.FloorToInt (transform.position.y / 10), Mathf.FloorToInt (transform.position.z / 10));
	}
}
