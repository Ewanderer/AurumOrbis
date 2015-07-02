using UnityEngine;
using System.Collections;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class FileHelper
{
	public static byte[] serializeObject<T>(T targetObject){
		MemoryStream s = new MemoryStream ();
		BinaryFormatter bf = new BinaryFormatter ();
		bf.Serialize (s, targetObject);
		return s.ToArray();
	}

	public static T deserializeObject<T>(byte[] input){
		MemoryStream s = new MemoryStream(input);
		BinaryFormatter bf = new BinaryFormatter ();
		T result = (T)bf.Deserialize (s);
		return result;
	}

	//Da Write to File keine Ordner erstellen kann, soll diese Funktion ganz fix auch größere Ordnerketten erstellen.
	public static void createPath(string Path){

	}

}

