using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Items sind keine Dauerhaften GameObjects sondern werden beim Spawnen(oder Droppen) in einen Container verpackt.
//Hier sind alle Informationen, sowohl RPG technisch, als auch das Design für den Container.
//Dies hier ist außerdem die Basisklasse für TWeapon und TEquipment
public class TItem :InteractiveObject {

	public TItem(string id):base(id){

	}

	public TItemStatistic OriginalItemStats;
	// Use this for initialization
}
