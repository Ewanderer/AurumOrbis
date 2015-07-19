using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class MyMsgType
{
	public static short LoginAnswer = MsgType.Highest + 1;
	public static short SkillUpdate= MsgType.Highest+2;
	public static short SystemError = MsgType.Highest + 3;

}

