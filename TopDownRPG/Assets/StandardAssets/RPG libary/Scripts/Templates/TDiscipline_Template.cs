using UnityEngine;
using System.Collections;

/**
 * \brief Diese Struktur dient der Verwaltung aller Vorlagen für Disziplinen,  aus ihr wird das Rohgerüst in der Creature generiert und später erweitert.
 * 
 */
public struct TDiscipline_Template {
	//Kopfdaten
	public string Name;/**<Name der Disziplin  */
	public string Description;/**<Beschreibung oder auch einfach Flavor-Text*/
	public string[] Prequisites;/**<Anforderung an potenzielle Interessierte, human readable, auch wenn in Kurzform für bessere Verwertung durch einen Checker */
	public int LearnCost;/**<Wie viel muss man als Neueinsteiger in Startguthaben/Sagenpunkteinvestieren */
	public float ExperienceModificator;/**<Alle in die Disziplin und seine Talente/Fertigkeiten eingehenden XP werden um diesen Wert modifiziert. */

}
