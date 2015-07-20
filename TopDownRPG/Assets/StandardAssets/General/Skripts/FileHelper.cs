using System.Collections;
using System.Collections.Generic;
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

	public static bool SanitizeForDirectory(ref string Path){
		string[] elements=Path.Split('/');
		if(elements[elements.Length-1].Contains(".")){
			Path="";
			for(int i=0;i<elements.Length-1;i++){
				Path+=elements[i]+"/";
			}
		}
		if (elements.Length == 1||Path==""||Path=="./")
			return false;
		return true;
	}

	//Da Write to File keine Ordner erstellen kann, soll diese Funktion ganz fix auch größere Ordnerketten erstellen.
	public static void createPath(string Path){
		if (!SanitizeForDirectory (ref Path))
			return;
		if (!Directory.Exists (Path)) {
			Directory.CreateDirectory(Path);
		}
	}

	public static void emptyPath(string Path){
		if (!SanitizeForDirectory (ref Path))
			return;
		try{
		Directory.Delete (Path,true);
		}finally{
		
		}
	}

	public static void WriteToFile(string Path,byte[] data){

		try{
		createPath (Path);
			using(FileStream fs =new FileStream(Path,FileMode.Create,FileAccess.Write)){
				foreach(byte b in data)
					fs.WriteByte(b);
			}
		}
		finally{

		}
	}

	public static byte[] ReadFromFile(string Path){
		List<byte> result=new List<byte>();
		try{
			using(FileStream fs = new FileStream(Path,FileMode.Open,FileAccess.Read)){
				int i = fs.ReadByte ();
				while (i!=-1) {
					result.Add((byte)i);
					i=fs.ReadByte();
				}
			}
		}
		finally{

		}
		return result.ToArray ();
	}

}

