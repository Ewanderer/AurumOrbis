using UnityEngine;
using System.Collections;


/**
 * Diese Klasse dient als Generierungsbasis für TEffect-Objekte und wird in einer Instanz im entsprechendem Katalog abgelegt
 * 
 */
public class TEffectBase
{
	public string Name;
	public Content[] Information;//Werden beim Benutzen der im RPG-Objekt vereinbarten Schnittstellen ausgegeben.
	public string GeneralCategory;//Manchmal ist es erforderlich, dass nicht nur die einzelnen passiven Effekte nicht stacken, sondern auch ganze Effekte nur einmalig auf dem Objekt exestieren können.
	public string Tags;//Dienen neben der GeneralCategory zur Klassifizierung eines Effekts(z.B. Fluch) haben aber keinen Einfluss auf das Stackverhalten.
	public int GeneralOrder;//Gleich oder höherwertige Effekte überschreiben den originalen Effekt. Ausgenommen hiervon sind Effekte mit negativen Wert. Diese können stacken und die Kategorie dient zur Identifizerung(Beispielsweise wenn man auf einem Schlag alle Flüche entfernen will)
	public string[] PassiveEffectStrings;//Siehe Effekt.odt
	public string[] ActiveEffectStrings;//Jeweils während der Laufzeit interpretierte Metaprogramme
	//public string[] ReactionEffectStrings;//Metaprogramme die allerdings situationabhängig ausgeführt werden
	public float oDuration;//Wenn die originale Dauer negativ ist, gilt der Effekt als Permament und Zeit wird nicht beachtet...

	public TEffectBase(string filestring){
		string[] textblocks = filestring.Split ('#');
		Name = textblocks [0];
		//Content out of string
		string[] contentblocks=textblocks[1].Split('~');
		Information=new Content[contentblocks.Length];
		for (int i=0; i<contentblocks.Length; i++) {
			Information[i]=new Content(contentblocks[i]);
		}
		GeneralCategory = textblocks [2];
		Tags = textblocks [3];
		GeneralOrder = System.Convert.ToInt16 (textblocks [4]);
		PassiveEffectStrings=textblocks[5].Split('~');
		ActiveEffectStrings=textblocks[6].Split('~');
		oDuration = System.Convert.ToSingle (textblocks [7]);
	}

}

