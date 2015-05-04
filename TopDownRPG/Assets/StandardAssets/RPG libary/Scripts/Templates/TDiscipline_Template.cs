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
	//Liste von Basistalenten/Ferigkeiten
	public string[] base_SkillHeads;/**<Liste mit jeweils ',' getrennten Parametern zur Initialisierung einer neuen Fertigkeit */
	public string[] base_PassiveEffects;/**<Liste mit passiven Effekten von denen der Besitzer Profetiert. */
	public string[] base_Talents;/**<Namesliste für alle Talente, die der Liste des Besitzers hinzugefügt werden*/
	//Liste von Erweiternedn Talenten/Fertigkeiten
	public string[] adv_SkillHeads;/**<Wie \see:base_SkillHeads, nur dass es vornerangestellt die Felder LvL-Requirment(Hauptrang in der Disziplin) und Kosten angegeben hat.*/
	public string[] adv_PassiveEffects;/**<Wie \see:base_PassiveEffects, nur dass es vornerangestellt die Felder LvL-Requirment(Hauptrang in der Disziplin) und Kosten angegeben hat.*/
	public string[] adv_Talents;/**<Wie \see:base_Talents, nur dass es vornerangestellt die Felder LvL-Requirment(Hauptrang in der Disziplin) und Kosten angegeben hat.*/
}
