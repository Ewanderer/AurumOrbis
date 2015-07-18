using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class TEffect:IRPGSource {
	public int ownID;
	public string Name;
	public string referenceID;
	public IRPGSource OriginalSource;//Für bestimmte Effekte, wie Bindungen ist es oft wichtig die originale Herkunft zu kennen
	public Content[] Information;//Werden beim Benutzen der im RPG-Objekt vereinbarten Schnittstellen ausgegeben.
	public string GeneralCategory;//Manchmal ist es erforderlich, dass nicht nur die einzelnen passiven Effekte nicht stacken, sondern auch ganze Effekte nur einmalig auf dem Objekt exestieren können.
	public string Tags;//Dienen neben der GeneralCategory zur Klassifizierung eines Effekts(z.B. Fluch) haben aber keinen Einfluss auf das Stackverhalten.
	public int GeneralOrder;//Gleich oder höherwertige Effekte überschreiben den originalen Effekt. Ausgenommen hiervon sind Effekte mit negativen Wert. Diese können stacken und die Kategorie dient zur Identifizerung(Beispielsweise wenn man auf einem Schlag alle Flüche entfernen will)
	public string[] PassiveEffectStrings;//Siehe Effekt.odt
	public List<string> WorkingPassiveEffectStrings;

	public string[] ActiveEffectStrings;//Jeweils während der Laufzeit interpretierte Metaprogramme
	EffectScriptObject[] scriptObjects;
	public EffectScriptObject[] ScriptObjects{
		get{return scriptObjects;}
	} 
	//public string[] ReactionEffectStrings;//Metaprogramme die allerdings situationabhängig ausgeführt werden
	public bool IsSupressed;//Bestimmte andere Effekte können zwar den Effekt nicht beenden, aber seine Wirkung einfrieren(Zeit läuft weiter)...
	public float oDuration;//Wenn die originale Dauer negativ ist, gilt der Effekt als Permament und Zeit wird nicht beachtet...
	public float cDuration;

	public void sendMessage(string Message,IRPGSource Source){
	
	}

	public TEffect(string id){
	
	}

	public TEffect(TEffectBase effectbase,IRPGSource source,RPGObject afflicted){
		Name = effectbase.Name;
		OriginalSource = source;
		Information = effectbase.Information;
		GeneralCategory = effectbase.GeneralCategory;
		Tags = effectbase.Tags;
		GeneralOrder = effectbase.GeneralOrder;
		PassiveEffectStrings = effectbase.PassiveEffectStrings;
		ActiveEffectStrings = effectbase.ActiveEffectStrings;
//		ReactionEffectStrings = effectbase.ReactionEffectStrings;
		oDuration = effectbase.oDuration;
		scriptObjects = new EffectScriptObject[ActiveEffectStrings.Length];
		for (int i=0; i<scriptObjects.Length; i++)
			scriptObjects [i] = new EffectScriptObject (ActiveEffectStrings [i], source, afflicted, this);
	}

	public string getID(){
		return referenceID;
	}

	public float this[string Valuename]{
		get{
			switch(Valuename){
			case "Name":
				return 0;
			default:
				if(Valuename.Split('.').Length>1){
					string[] VS=Valuename.Split('.');
					int E=System.Convert.ToInt16(VS[0]);
					if(VS[1][0]=='%')
						return scriptObjects[E].nvalues.Find(delegate(EffectScriptObject.NumericValue obj) {
							return obj.Name==VS[1].TrimStart('%');
						}).Value;
					else
					if(VS[1][0]=='§'){
						string bs="";
						for(int i=2;i<VS.Length;i++)
							bs+=VS[i]+".";
						bs.TrimEnd('.');
						switch(VS[1].TrimStart('§')){
							case "effect":
							return scriptObjects[E].effect[bs];
						
						case "afflicted":
							return scriptObjects[E].afflicted[bs];
						
						case "source":
							return scriptObjects[E].source[bs];
						}
					}else{
						return 0;
					}
				}else
					return 0;
					break;
			}
			return 0;
		}
	}

}
