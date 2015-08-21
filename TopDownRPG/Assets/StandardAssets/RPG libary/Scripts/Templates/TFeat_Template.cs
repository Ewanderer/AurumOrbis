using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class TFeat_Template
{
	public string Name;
	public string shortDescription;
	public string longDescription;
	public bool isActivateable;
	public TEffect linkendEffect;//Dieser Effekt wird mit Erhalt des Talents angefügt...
	public string activeActionName;
	[System.NonSerialized]
	private Executable _activeAction;//Diese Aktion wird beim aktivieren freigesetzt...
	public Executable activeAction{
		get{
			if(_activeAction!=null&&_activeAction.name==activeActionName)
				return activeAction;
			return null;
		}
	}
}

