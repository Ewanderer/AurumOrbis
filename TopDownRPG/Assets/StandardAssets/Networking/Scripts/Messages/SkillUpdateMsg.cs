using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class SkillUpdateMsg : MessageBase
{
	public RPGObject.Skill[] _skills;
	public override void Deserialize (NetworkReader reader)
	{
		_skills=FileHelper.deserializeObject<RPGObject.Skill[]>(reader.ReadBytesAndSize ());
	}

	public override void Serialize (NetworkWriter writer)
	{
		writer.WriteBytesFull (FileHelper.serializeObject (_skills));
	}

	public SkillUpdateMsg(RPGObject.Skill[] skills){
		_skills = skills;
	}

	public SkillUpdateMsg(){}

}

