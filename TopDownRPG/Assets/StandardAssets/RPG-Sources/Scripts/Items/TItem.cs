using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//Items sind keine Dauerhaften GameObjects sondern werden beim Spawnen(oder Droppen) in einen Container verpackt.
//Hier sind alle Informationen, sowohl RPG technisch, als auch das Design für den Container.
//Dies hier ist außerdem die Basisklasse für TWeapon und TEquipment
public class TItem :RPGObject {
	
	public TItemStatistic OriginalItemStats;

	
	//Diese Funktion gibt wieder ob das Objekt versteckt ist(return bool) und zusätzliche Informatioen wie diese Tarnung aufgebaut ist.
	public override bool IsVisible(out int HideValue,out TEffect[] UseEffects){
		
	}
	
	//Das Objekt macht Auskunft über die Verfügbarkeit einer Eigenschaft und bei numerischen Werten Auskunft über die Höhe des gefragten bestimmten Wertes. Dabei werden alle Informationen wie sich der Wert zusammengesetzt mitgegeben.
	public override bool CheckValue (string NameOfValue,out int BaseValue, out int EndValue, out TEffect[] UseEffects){
		
	}


	public override float RecieveDamage (float Value, string Typ){
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
