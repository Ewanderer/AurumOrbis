using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EffectScriptObject
{

	bool isActive;

	public bool IsActive {
		get{ return isActive && !Effect.IsSupressed;}
	}

	//Konstanten
	IRPGSource Source;//Wird aus TEffect Source übernommen
	RPGObject Afflicted;
	TEffect Effect;

	public IRPGSource source {
		get{ return Source;}
	}

	public RPGObject afflicted {
		get{ return Afflicted;}
	}

	public TEffect effect {
		get{ return Effect;}
	}

	//Variablen

	public class StringValue
	{
		public string Name;
		public string Value;

		public StringValue (string name, string value)
		{
			Name = name;
			Value = value;
		}
	}

	public class NumericValue
	{
		public string Name;
		public float Value;

		public NumericValue (string name, float value)
		{
			Name = name;
			Value = value;
		}
	}

	List<StringValue> sValues = new List<StringValue> ();
	List<NumericValue> nValues = new List<NumericValue> ();

	public List<NumericValue> nvalues {
		get{ return nValues;}
	}

	//Triggerschalter
	bool AllowActivate;
	bool UseTimer;
	float TimeIntervall;
	float Timer;
	bool TakeDamage;
	bool RecieveHealing;
	//Trigger-Interface Funktionen

	public struct Paramter
	{
		public string Name;
		public IRPGSource O;

		public Paramter (string _Name, IRPGSource _Object)
		{
			Name = _Name;
			O = _Object;
		}
	}

	public void OnActivate (IRPGSource s)
	{
		if (!Effect.IsSupressed && AllowActivate)
			ExecuteScript (null);
		
	}

	public void OnTakeDamage(ref float value, string Type,IRPGSource source){
		if (!Effect.IsSupressed && TakeDamage) {
			NumericValue v=new NumericValue("value",value);
			nValues.Add(v);
			StringValue t=new StringValue("type",Type);
			sValues.Add(t);
			Paramter s=new Paramter("source",source);
			ExecuteScript(new Paramter[]{s});
			nValues.Remove(v);
			sValues.Remove(t);
			value=v.Value;
		}
	}

	public void OnRecieveHealing(ref float value,IRPGSource source){
		if (!Effect.IsSupressed && RecieveHealing) {
			NumericValue v=new NumericValue("value",value);
			nValues.Add(v);
			Paramter s=new Paramter("source",source);
			ExecuteScript(new Paramter[]{s});
			nValues.Remove(v);
			value=v.Value;
		}
	}


	//
	List<string> Script = new List<string> ();

	//Benötige evt. verschachtelte If Blöcke oder so

	//Funktion zum Interpretieren der Scripts
	void ExecuteScript (params Paramter[] Parameters)
	{
		bool SkipBlock = false;
		bool SkipElse = false;
		int PC = 0;
		while (Script[PC]!="done"&&PC<Script.Count) {
			string[] split = Script [PC].Split (' ');
			if (SkipElse && split [0] == "else") {
				SkipBlock = true;
				SkipElse = false;
				PC++;
				continue;
			}
			if (SkipBlock && (split [0] == "endelse" || split [0] == "else")) {
				SkipBlock = false;
				PC++;
				continue;
			}		
			if (split [0] == "skip") {
				SkipBlock = true;
				PC++;
				continue;
			}
			if (split [0] == "goto") {
				PC = System.Convert.ToInt16 (split [2]);
				continue;
			}
			if (split [0] == "if") {
				bool IsTrue = true;
				//Split Expressions
				List<string[]> Expressions = new List<string[]> ();
				List<string> Operators = new List<string> ();
				List<string> currentexpression = new List<string> ();
				for (int i=1; i<split.Length; i++) {
					if (split [i] == "&" || split [i] == "|") {
						Expressions.Add (currentexpression.ToArray ());
						currentexpression.Clear ();
						Operators.Add (split [i]);
					} else {
						currentexpression.Add (split [i]);
					}
				}
				//Check all Expressions
				List<bool> expressionstate = new List<bool> ();
				foreach (string[] expression in Expressions) {
					//Aussagen die Anhand der Länge von expression getroffen werden können
					//1=Abfage nach boolschem Wert
					//2=Negierung eines boolschen Wertes
					//3=Vergleich von 2 Werten
					//4=Invertierung des Vergleichs von 2 Werten
					switch (expression.Length) {
					case 1:
						expressionstate.Add (CollectObjectbooleanValue (expression [0]));
						break;
					case 2:
						expressionstate.Add (!CollectObjectbooleanValue (expression [0]));
						break;
					case 3:
						if (expression [0] [0] == '%' && expression [2] [0] == '%') {
							float a = CollectObjectnumericValue (expression [0]);
							float b = CollectObjectnumericValue (expression [2]);
							switch (expression [1]) {
							case "<":
								if (a < b)
									expressionstate.Add (true);
								else
									expressionstate.Add (false);
								break;
							case ">":
								if (a > b)
									expressionstate.Add (true);
								else
									expressionstate.Add (false);
								break;
							case "==":
								if (a == b)
									expressionstate.Add (true);
								else
									expressionstate.Add (false);
								break;
							case "!=":
								if (a != b)
									expressionstate.Add (true);
								else
									expressionstate.Add (false);
								break;
							default:
								RPGLogger.LogEvent ("ungueltiger ausdruck:" + expression [1], Effect.Name, true);
								expressionstate.Add (false);
								break;
							}
						} else {
							if (expression [0] [0] == '$' && expression [2] [0] == '$') {
								string a = sValues.Find (delegate(StringValue obj) {
									return obj.Name == expression [0].TrimStart ('$');
								}).Value;
								string b = sValues.Find (delegate(StringValue obj) {
									return obj.Name == expression [2].TrimStart ('$');
								}).Value;
								switch (expression [1]) {
								case "==":
									if (a == b)
										expressionstate.Add (true);
									else
										expressionstate.Add (false);
									break;
								case "!=":
									if (a != b)
										expressionstate.Add (true);
									else
										expressionstate.Add (false);
									break;
								}
							} else {
								RPGLogger.LogEvent ("Ungültiger boolscher Ausdruck in Zeile:" + PC.ToString (), Effect.Name, true);
								expressionstate.Add (false);
							}
						}
						break;
					case 4:
						if (expression [0] [0] == '%' && expression [2] [0] == '%') {
							float a = CollectObjectnumericValue (expression [0]);
							float b = CollectObjectnumericValue (expression [2]);
							switch (expression [1]) {
							case "<":
								if (a < b)
									expressionstate.Add (!true);
								else
									expressionstate.Add (!false);
								break;
							case ">":
								if (a > b)
									expressionstate.Add (!true);
								else
									expressionstate.Add (!false);
								break;
							case "==":
								if (a == b)
									expressionstate.Add (!true);
								else
									expressionstate.Add (!false);
								break;
							case "!=":
								if (a != b)
									expressionstate.Add (!true);
								else
									expressionstate.Add (!false);
								break;
							default:
								RPGLogger.LogEvent ("ungueltiger ausdruck:" + expression [1], Effect.Name, true);
								expressionstate.Add (false);
								break;
							}
						} else {
							if (expression [0] [0] == '$' && expression [2] [0] == '$') {
								string a = sValues.Find (delegate(StringValue obj) {
									return obj.Name == expression [0].TrimStart ('$');
								}).Value;
								string b = sValues.Find (delegate(StringValue obj) {
									return obj.Name == expression [2].TrimStart ('$');
								}).Value;
								switch (expression [1]) {
								case "==":
									if (a == b)
										expressionstate.Add (!true);
									else
										expressionstate.Add (!false);
									break;
								case "!=":
									if (a != b)
										expressionstate.Add (!true);
									else
										expressionstate.Add (!false);
									break;
								}
							} else {
								RPGLogger.LogEvent ("Ungültiger boolscher Ausdruck in Zeile:" + PC.ToString (), Effect.Name, true);
								expressionstate.Add (false);
							}
						}
						break;
					}
				}

				//Verbinde den gesamten boolschen Ausdruck
				int leftoverparameter = Operators.Count;//Anzahl der übrig gebliebenden Operatoren
				int leftoverands = Operators.FindAll (delegate(string obj) {
					return obj == "&" || obj == "|";
				}).Count;
	
				while (leftoverparameter>0) {
					for (int i=0; i<Operators.Count; i++) {
						if (leftoverands > 0) {
							if (Operators [i] != "&")
								continue;
							IsTrue = IsTrue && expressionstate [i] && expressionstate [i + 1];
							Operators [i] = "0";//Verhindere die doppelte Abarbeitung von Ausdrücken 
							leftoverands--;
						} else {
							if (Operators [i] != "0") {
								IsTrue &= expressionstate [i] || expressionstate [i + 1];
								Operators [i] = "0";//Verhinder die doppelte Abarbeitung von Ausdrücken
								leftoverparameter--;
							}
						}

					}
				}
				//Festlegung über die Flussrichtung der nächsten Blöcke
				SkipBlock = !IsTrue;
				SkipElse = IsTrue;
				PC++;
				continue;
			}

			//Allgemeine Interpretation von Kommandos
			switch (split [0] [0]) {
			case '§':
				//Zugriff auf eine der Konstanten Objekten
				if (split [0].Split ('.') [0] == "§effect") {
					ProcessCommandToEffect (Effect, split [0].Split ('(') [1].TrimEnd (')').Split (','), Parameters);
				} else
				if (split [0].Split ('.') [0] == "§afflicted") {
					ProcessCommandToObject (Afflicted, split [0].Split ('(') [1].TrimEnd (')').Split (','), Parameters);
				} else {
					IRPGSource target = (new List<Paramter> (Parameters)).Find (delegate(Paramter obj) {
						return obj.Name == split [0].Split ('.') [0].TrimStart ('§');
					}).O;
					if (target != default(IRPGSource)) {
						if (target is TCreature)
							ProcessCommandToObject (target as TCreature, split [0].Split ('(') [1].TrimEnd (')').Split (','), Parameters);
						else
							if (target is TEffect)
							ProcessCommandToEffect (target as TEffect, split [0].Split ('(') [1].TrimEnd (')').Split (','), Parameters);
					}

				}
				break;
			case '$':
				//Edit Strings
				StringValue sv;
				if((sv=sValues.Find(delegate(StringValue obj) {
					return obj.Name==split[0].TrimStart('$');
				}))!=default(StringValue)){
					switch(split[1]){
					case "=":
						sv.Value=split[2];
						break;
					case "+=":
						sv.Value+=split[2];
						break;
					}
				}else{
					RPGLogger.LogEvent("Value:"+split[0]+"not found","RPG-ScriptObject",true);

				}

				break;
			case '%':
				NumericValue nv;
				//Edit Numeric Values
				if((nv=nValues.Find(delegate(NumericValue obj) {
					return obj.Name==split[0].TrimStart('%');
				}))!=default(NumericValue)){
					switch(split[1]){
					case "=":
						nv.Value=System.Convert.ToSingle(split[2]);
						break;
					case "+=":
						nv.Value+=System.Convert.ToSingle(split[2]);
						break;
					case "-=":
						nv.Value-=System.Convert.ToSingle(split[2]);
						break;
					case "*=":
						nv.Value*=System.Convert.ToSingle(split[2]);
						break;
					case "/=":
						nv.Value/=System.Convert.ToSingle(split[2]);
						break;
					}

				}else{
					RPGLogger.LogEvent("Value:"+split[0]+"not found","RPG-ScriptObject",true);
				}
				break;
			}

			PC++;
		}
	}
	

	void ProcessCommandToObject (RPGObject target, string[] CallParameter, Paramter[] Triggerparameter)
	{
	
	}

	void ProcessCommandToEffect (TEffect target, string[] CallParameter, Paramter[] Triggerparameter)
	{
	
	}

	float CollectObjectnumericValue (string O, params Paramter[] Parameters)
	{
		
		switch (O [0]) {
		case '§':
			switch (O.Split ('.') [0].TrimStart ('§')) {
			case "afflicted":
				if (Afflicted != null)
					return Afflicted [O.Split ('.') [1]];
				return 0;
			case "effect":
				if (Effect != null)
					return Effect [O.Split ('.') [1]];
				else
					return 0;
			default:
				return (new List<Paramter> (Parameters)).Find (delegate(Paramter obj) {
					return obj.Name == O.Split ('.') [0].TrimStart ('§');
				}).O [O.Split ('.') [1]];
			}
		case '$':
			RPGLogger.LogEvent ("Ein String kann nicht in numerischen Wert umgewandelt werden.", Effect.Name, true);
			return 0;
		case '%':
			if (nValues.Exists (delegate(NumericValue obj) {
				return obj.Name == O.TrimStart ('%');
			}))
				return nValues.Find (delegate(NumericValue obj) {
					return obj.Name == O.TrimStart ('%');
				}).Value;
			return 0;
		}
		return 0;
	}

	bool CollectObjectbooleanValue (string O, params Paramter[] Parameters)
	{

		switch (O [0]) {
		case '§':
			switch (O.Split ('.') [0].TrimStart ('§')) {
			case "afflicted":
				if (Afflicted [O.Split ('.') [1]] > 0)
					return true;
				return false;
			case "effect":
				if (Effect [O.Split ('.') [1]] > 0)
					return true;
				else
					return false;
			default:
				if ((new List<Paramter> (Parameters)).Exists (delegate(Paramter obj) {
					return obj.Name == O.Split ('.') [0].TrimStart ('§');
				}))
					return (new List<Paramter> (Parameters)).Find (delegate(Paramter obj) {
						return obj.Name == O.Split ('.') [0].TrimStart ('§');
					}).O [O.Split ('.') [1]] > 0;
				else
					return false;
			}
		case '$':
			RPGLogger.LogEvent ("Ein String kann nicht in boolschen Wert umgewandelt werden.", Effect.Name, true);
			return false;
		case '%':
			if (nValues.Find (delegate(NumericValue obj) {
				return obj.Name == O.TrimStart ('%');
			}).Value > 0)
				return true;
			return false;
		}
		return false;
	}

	//Indikatioren für Varibeltypen
	//§=TriggerValue,constantValue
	//$=StringValue
	//%=NumericValue

	//Aufbau des Sykripts
	//Trigger
	//Variabel
	//Skript

	public EffectScriptObject (string _Script, IRPGSource _Source, RPGObject _Target, TEffect _Effect)
	{
		string[] Lines = _Script.Split (';');
		int state = 0;
		for (int i=0; i<Lines.Length; i++) {
			Lines [i] = Lines [i].ToLower ();
			if (Lines [i] == "endblock") {
				state++;
				continue;
			}
			if (state == 0) {
				//Trigger
			} else
			if (state == 1) {
				//Variablen
				string varstring = Lines [i];
				if (varstring [0] == '$') {
					sValues.Add (new StringValue (varstring.Split ('=') [0].TrimStart ('$'), varstring.Split ('=') [1]));
				}
				if (varstring [0] == '%') {
					nValues.Add (new NumericValue (varstring.Split ('=') [0].TrimStart ('%'), System.Convert.ToInt16 (varstring.Split ('=') [1])));
				}
			} else
			if (state == 2) {
				//Script
				Script.Add (Lines [i]);
			}
		}
	}
	
	//Funktion zur Neuverknüpfung des Effekts
	public void SetUpEffect (IRPGSource _Source, RPGObject _Target)
	{
		Source = _Source;
		Afflicted = _Target;
	}


}
