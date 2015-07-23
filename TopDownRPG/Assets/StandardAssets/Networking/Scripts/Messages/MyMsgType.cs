using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public static class MyMsgType
{
	public static short LoginAnswer = MsgType.Highest + 1;
	public static short SkillUpdate= MsgType.Highest+2;
	public static short SystemError = MsgType.Highest + 3;
	public static short RequestVisibility=MsgType.Highest+4;
	public static short LoginRequest=MsgType.Highest+5;
	public static short NewAccountRequest = MsgType.Highest + 6;
	public static short HookChar = MsgType.Highest + 7;
	public static short AddEffect = MsgType.Highest + 8;
	public static short RecieveDamage = MsgType.Highest + 9;
	public static short RemoveEffect=MsgType.Highest+10;
	public static short CreateObjectFromID = MsgType.Highest + 11;
	public static short CreateObjectFromTemplate = MsgType.Highest + 12;

}

