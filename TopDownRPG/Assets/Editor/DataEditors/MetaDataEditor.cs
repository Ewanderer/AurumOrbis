using UnityEngine;
using UnityEditor;
using System.Collections;

public class MetaDataEditor : EditorWindow
{

	public MetaDataManager manager;
	public string SettingName = "AurumOrbis";

	[MenuItem("TopDownRPG/MetaDataEditor")]
	public static void Init ()
	{
		if (EditorApplication.SaveCurrentSceneIfUserWantsTo ()) {
			MetaDataEditor window = EditorWindow.CreateInstance<MetaDataEditor> ();
			EditorApplication.NewEmptyScene ();
			GameObject go = new GameObject ();
			window.manager = go.AddComponent<MetaDataManager> ();
			window.Show ();
		}
	}

	void OnDestroy ()
	{
		manager.SaveToFile (SettingName);
		DestroyImmediate (manager);
	}

	//Generelle Optionen
	int _mode = 0;//0-nothing,1-effects,2-feats
	bool showSearchResults = false;
	int selection = 0;
	string searchedString = "";

	int mode {
		get {
			return _mode;
		}
		set {
			_mode = value;
			showSearchResults = false;
			searchedString = "";
			selection = 0;
		}
	}

	//Effektspezische Werte
	bool showPassiveStrings;


	void OnGUI ()
	{
		SettingName = EditorGUILayout.TextField ("Setting:", SettingName);
		EditorGUILayout.BeginHorizontal ();
		if (GUILayout.Button ("Speichern"))
			manager.SaveToFile (SettingName);
		if (GUILayout.Button ("Laden"))
			manager.LoadFromFile (SettingName);
		EditorGUILayout.EndHorizontal ();

		if (GUILayout.Button ("Effekte"))
			mode = 1;
		if (mode == 0)
			return;
		EditorGUILayout.Separator ();
		switch (mode) {
		case 1:
			//Navigation
			EditorGUILayout.BeginHorizontal ();
			if (GUILayout.Button ("<"))
				selection--;
			if (GUILayout.Button (">"))
				selection++;
			if (selection < 0)
				selection = manager.allEffects.Count - 1;
			if (selection >= manager.allEffects.Count)
				selection = 0;
			EditorGUILayout.EndHorizontal ();
			searchedString = EditorGUILayout.TextField ("Suche:", searchedString);
			showSearchResults = EditorGUILayout.Foldout (showSearchResults, "Treffer:");
			if (showSearchResults) {
				TEffect[] e = manager.allEffects.FindAll (delegate(TEffect obj) {
					return obj.Name.Contains (searchedString);
				}).ToArray ();
				for (int i=0; i<e.Length; i++) {
					if (GUILayout.Button (e [i].Name)) {
						selection = manager.allEffects.IndexOf(e[i]);
						showSearchResults = false;
					}
				}
			}
			if (GUILayout.Button ("Neuer Effekt")) {
				manager.allEffects.Add (new TEffect ());
				selection = manager.allEffects.Count - 1;
			}
			//Anzeige des eigentlichen Effekts
			if (manager.allEffects.Count > 0) {
				TEffect sE = manager.allEffects [selection];
				sE.Name = EditorGUILayout.TextField ("Name:", sE.Name);
				sE.GeneralCategory = EditorGUILayout.TextField ("Kategorie:", sE.GeneralCategory);
				sE.Tags = EditorGUILayout.TextField ("Tags:", sE.Tags);
				sE.GeneralOrder = (int)EditorGUILayout.FloatField ("Ordung:", sE.GeneralOrder);
				sE.oDuration = EditorGUILayout.FloatField ("Dauer:", sE.oDuration);



				if(GUILayout.Button("Effekt Löschen")){
					selection=0;
					manager.allEffects.Remove(sE);
				}
			}

			break;
		}
	}

}
