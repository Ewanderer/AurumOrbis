using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//Dieser Lustige Skript stellt den Settingspezifischen Content, von Völkern über Professionen, Feats, Effekten bis hin zu den Objekt-Templates

public class MetaDataManager : MonoBehaviour
{

	public static MetaDataManager instance;

	public List<TEffect> allEffects=new List<TEffect>();
	public List<TFeat_Template> allFeats=new List<TFeat_Template>();
	public List<TProfession_Template> allProfessions=new List<TProfession_Template>();
	public List<TCreature_Template> allPeoples=new List<TCreature_Template>();



	// Use this for initialization
	void Start ()
	{
		instance = this;
	}

	public void LoadFromFile(string SettingName){
		allEffects=FileHelper.deserializeObject<List<TEffect>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/effects.obj"));
		allFeats=FileHelper.deserializeObject<List<TFeat_Template>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/feats.obj"));
		allProfessions=FileHelper.deserializeObject<List<TProfession_Template>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/professions.obj"));
		allPeoples=FileHelper.deserializeObject<List<TCreature_Template>> (FileHelper.ReadFromFile("./data/settings/"+SettingName+"/peoples.obj"));
	}

	public void SaveToFile(string SettingName){
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/effects.obj", FileHelper.serializeObject<List<TEffect>> (allEffects));
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/feats.obj", FileHelper.serializeObject<List<TFeat_Template>> (allFeats));
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/professions.obj", FileHelper.serializeObject<List<TProfession_Template>> (allProfessions));
		FileHelper.WriteToFile ("./data/settings/" + SettingName + "/peoples.obj", FileHelper.serializeObject<List<TCreature_Template>> (allPeoples));
	}
}

