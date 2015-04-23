using UnityEngine;
using System.Collections;

public static class RPGLogger{

	public static string LoggedSources;

	

	public static void LogEvent(string Message,string Source,bool IsError){
		if (IsError || LoggedSources.Contains (Source)) {
			if(IsError){
				Debug.LogError("RPG Libary reports Error:"+Source+":"+Message);

			}

		}
	}

}
