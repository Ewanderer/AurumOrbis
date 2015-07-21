using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Dieser Lustige Skript stellt den Settingspezifischen Content, von Völkern über Professionen, Feats, Effekten bis hin zu den Objekt-Templates

public class MetaDataManager : MonoBehaviour
{

	public static MetaDataManager instance;

	public List<TEffect> allEffects=new List<TEffect>();
	public List<TFeat> allFeats=new List<TFeat>();
	public List<TProfession> allProfessions=new List<TProfession>();
	public List<TPeople> allPeoples=new List<TPeople>();



	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	public void LoadFromFile(string SettingName){
		allEffects=FileHelper.deserializeObject<List<TEffect>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/effects.obj"));
		allFeats=FileHelper.deserializeObject<List<TFeat>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/feats.obj"));
		allProfessions=FileHelper.deserializeObject<List<TProfession>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/professions.obj"));
		allPeoples=FileHelper.deserializeObject<List<TPeople>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/peoples.obj"));
	}

	public void SaveToFile(string SettingName){
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/effects.obj", FileHelper.serializeObject<List<TEffect>> (allEffects));
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/feats.obj", FileHelper.serializeObject<List<TFeat>> (allFeats));
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/professions.obj", FileHelper.serializeObject<List<TProfession>> (allProfessions));
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/peoples.obj", FileHelper.serializeObject<List<TPeople>> (allPeoples));
	}
}

