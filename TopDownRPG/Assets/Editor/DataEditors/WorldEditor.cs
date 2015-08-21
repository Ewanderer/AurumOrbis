using UnityEngine;
using UnityEditor;
using System.Collections;

public class WorldEditor : EditorWindow {

	private StaticWorldManager manager;

	[MenuItem("TopDownRPG/WorldEditor")]
	public static void Init(){
		if (EditorApplication.SaveCurrentSceneIfUserWantsTo ()) {
			EditorApplication.OpenScene("EditorEnviorment.unity");
		WorldEditor window = EditorWindow.CreateInstance<WorldEditor> ();
		window.manager=GameObject.FindObjectOfType<StaticWorldManager>();
		window.Show ();
		
		}
	}

	bool foldoutGeneral=false;
	string choosenWorldName="Aurum Orbis";

	void OnGUI(){
		foldoutGeneral = EditorGUILayout.Foldout (foldoutGeneral, "Allgemeines");
		if (foldoutGeneral) {
			if(GUILayout.Button("Neue Welt, bereinigt die gesamte Welt")){
				foreach(StaticObject obj in GameObject.FindObjectsOfType<StaticObject>())
					Destroy(obj);
			}
			choosenWorldName=EditorGUILayout.TextField("Weltname:",choosenWorldName);
			if(GUILayout.Button("Welt speichern"))
				manager.saveWorld(choosenWorldName);
			if(GUILayout.Button("Welt laden"))
				manager.setUpWorld(choosenWorldName);
			if(GUILayout.Button("Welt aufsetzten")){
				foreach(StaticObject obj in GameObject.FindObjectsOfType<StaticObject>())
					Destroy(obj);
				foreach(TWorld.WorldNode node in manager.usedWorld.Nodes){
					node.unload();
					node.load(true);
				}
			}
		}
	}
}
