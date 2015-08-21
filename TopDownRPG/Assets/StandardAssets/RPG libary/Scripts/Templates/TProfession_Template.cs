using System.Collections;
using System.Collections.Generic;

public struct TProfession_Template
{
	//Kopfdaten
	public string Name;/**<Name der Disziplin  */
	public string Description;/**<Beschreibung oder auch einfach Flavor-Text*/
	public string[] Prequisites;/**<Anforderung an potenzielle Interessierte, human readable, auch wenn in Kurzform für bessere Verwertung durch einen Checker */
	public int LearnCost;/**<Wie viel muss man als Neueinsteiger in Startguthaben/Sagenpunkteinvestieren */
	public float ExperienceModificator;/**<Alle in die Disziplin und seine Talente/Fertigkeiten eingehenden XP werden um diesen Wert modifiziert. */

	public List<TFeat_Template> RankFeat;//Diese Talente werden jeweils mit erreichen der entsprechenden Stufe gewährt.
	public List<List<TFeat_Template>> Feats;//Liste von Talenten die auf dem jeweiligem Rang erhältlich sind.


}

