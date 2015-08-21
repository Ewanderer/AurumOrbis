
using System.Collections;
[System.Serializable]
public class TCreature_Template
{
	public string Name;
	public string Description;
	[System.NonSerialized]
	public TEffect linkedEffect;//Dieser Effekt wird allen angehörigen dieser Art angerechnet.
	public TCreatureLimb rootLimb;
	public SizeCategory size;
}
[System.Serializable]
public struct TCreatureLimb{
	public string name;


	public string equipmentName;//Art welche Art von Equipment hier angeelgt werden darf
	public SizeCategory size;//Für Beschränkungen von Equipment
	public int weaponSize;//Gibt auskunft ob und wie groß eine Waffe sein kann. 0=keine Waffe.
	public int pairNr;//Für zweihändige Waffen benötigt man zwei geeignete mit selben PairNr.-Wert Limbs.

	public float vital;//Vitale Punkte kosten jeweils einen festen Teil an der maximalen Gesundheit bei Verlust. Ist dieser wert 1, bedeutet der Verlust Tod
	public float regrowthRate;//Anzahl der Rasten bis dieses Limb regeniert wurde. -1 kann nicht regenerieren.


	public TCreatureLimb[] subLimbs;//Um die Struktur einer Kreatur gut wiederzugeben werden alle Limbs an einen Knotenpunkt gehängt. 
}

