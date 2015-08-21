
public struct TSkill_Template
{
	public string Name;
	public string shortDescription;
	public string longDescription;
	public int Category;//0-Unsorted, 1-Social, 2-Crafting, 3-Knowlegde, 4-Combat. Bei Combat wird die Wertberechnung um die Attribute erweitert :D
	public int learnComplexityClass;//Erfahrung wird mit ComplexityClass*0.25 Multipliziert.
	public string attribute1;
	public string attribute2;
	public string attribute3;
	public string[] counterSkills;//"Skillname/SkillName(Fokus)" "Multiplikator für diesen Wert"
	public string[] replaceSkills;//Ähnlich wie Counter ledeglich dass man Ersatz schafft.
	public int notLearnedMalus;//Wenn nichts gelernt wurde, wird dieser Malus erteilt, wenn -1 dann gilt automatischer Fehlschlag...
	//Foki
	public string[] skillFokusName;
	public string[] skillFokusAttributes;//Immer 3 Attribute mit , getrennt
	public string[][] skillFokusCounter;
	public int[] skillFokusComplexityMalus;//Wenn eine Figur nicht über den Fokus verfügt, wird de Probe um diesen Wert erschwert
}


