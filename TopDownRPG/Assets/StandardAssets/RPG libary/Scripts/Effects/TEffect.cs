using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TEffect:ScriptableObject,IRPGSource {
	public string Name;
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
	public string[] ReactionEffectStrings;//Metaprogramme die allerdings situationabhängig ausgeführt werden
	public bool IsSupressed;//Bestimmte andere Effekte können zwar den Effekt nicht beenden, aber seine Wirkung einfrieren(Zeit läuft weiter)...
	public float oDuration;//Wenn die originale Dauer negativ ist, gilt der Effekt als Permament und Zeit wird nicht beachtet...
	public float cDuration;
}
