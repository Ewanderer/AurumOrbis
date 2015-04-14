using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectScriptObject {

	bool isActive;
	public bool IsActive{
		get{return isActive&&!Effect.IsSupressed;}
	}

	//Konstanten
	IRPGSource Source;//Wird aus TEffect Source übernommen
	RPGObject Afflicted;
	TEffect Effect;

	//Variablen

	public class StringValue
	{
		public string Name;
		public string Value;
		public StringValue(string name,string value){
			Name=name;
			Value=value;
		}
	}

	public class NumericValue
	{
		public string Name;
		public float Value;
	}

	List<StringValue> sValues=new List<StringValue>();
	List<NumericValue> nValues = new List<NumericValue> ();

	//Triggerschalter
	bool AllowActivate;
	bool UseTimer;
	float TimeIntervall;
	float Timer;
	bool TakeDamageCreature;
	bool TakeDamageEffect;
	bool TakeHealCreature;
	bool TakeHealEffect;
	//Trigger-Interface Funktionen

	public struct Paramter
	{
		string Name;
		object O;
		public Paramter(string _Name,object _O){
			Name=_Name;
			O=_O;
		}
	}

	public void OnActivate(IRPGSource s){
		if (!Effect.IsSupressed && AllowActivate)
			ExecuteScript (new Paramter ());
		
	}

	//
	List<string> Script = new List<string> ();

	//Funktion zum Interpretieren der Scripts

	void ExecuteScript(params Paramter[] Parameters){
	
	}

	//Indikatioren für Varibeltypen
	//§=TriggerValue
	//$=StringValue
	//%=NumericValue

	//Aufbau des Sykripts
	//Trigger
	//Variabel
	//Skript

	public EffectScriptObject(string _Script,IRPGSource _Source,RPGObject _Target,TEffect _Effect){
		string[] Lines=_Script.Split(';');
		int state = 0;
		for (int i=0; i<Lines.Length; i++) {
			Lines[i]=Lines[i].ToLower();
			if(Lines[i]=="endblock"){
				state++;
				continue;
			}
			if(state==0){
				//Trigger
			}else
			if(state==1){
				//Variablen
				string varstring=Lines[i];
				if(varstring[0]=='$'){
					sValues.Add(new StringValue(varstring.Split('=')[0].TrimStart('$'),varstring.Split('=')[1]));
				}
				if(varstring[0]=='%'){
					sValues.Add(new StringValue(varstring.Split('=')[0].TrimStart('$'),System.Convert.ToInt16(varstring.Split('=')[1])));
				}
			}else
			if(state==2){
				//Script
				Script.Add(Lines[i]);
			}
		}
	}
	
	//Funktion zur Neuverknüpfung des Effekts
	public void SetUpEffect(IRPGSource _Source,RPGObject _Target){
		Source = _Source;
		Afflicted = _Target;
	}


}
