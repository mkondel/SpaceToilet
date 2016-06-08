﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class Persister : MonoBehaviour {

	public static Persister persister;
	public string dataFileName = "savedSettings.data";
	public CustomGameSettings settingsOfTheGame;

	public void setMainVolume(float newLvl){
		settingsOfTheGame.mainVolume = newLvl;
	}
	public void setMusicVolume(float newLvl){
		settingsOfTheGame.musicVolume = newLvl;
	}
	public void setFxVolume(float newLvl){
		settingsOfTheGame.fxVolume = newLvl;
	}
	public void setKhVolume(float newLvl){
		settingsOfTheGame.khVolume = newLvl;
	}
	public void setExplosionsVolume(float newLvl){
		settingsOfTheGame.explosionsVolume = newLvl;
	}
	public void setBuzzVolume(float newLvl){
		settingsOfTheGame.buzzVolume = newLvl;
	}
	public void setWinningVolume(float newLvl){
		settingsOfTheGame.winningVolume = newLvl;
	}
	public void setLoosingVolume(float newLvl){
		settingsOfTheGame.loosingVolume = newLvl;
	}


	private string pathToSaveFile;

	// Use this for initialization
	void Awake () {
		Debug.Log ("Awake in Persister()");
		pathToSaveFile = Application.persistentDataPath + "/" + dataFileName;
		Debug.Log ("pathToSaveFile = "+pathToSaveFile);
		if (persister == null) {
			Debug.Log ("persister is null");
			DontDestroyOnLoad (gameObject);
			persister = this;
			LoadSettings ();
		} else if (persister != this) {
			Debug.Log ("persister is not null and not this.  Destroying self.");
			Destroy (gameObject);
		}
	}

	void OnApplicationQuit(){
		Debug.Log ("Saving settings because app is quitting...");
		SaveSettings ();
	}

	void SaveSettings(){
		Debug.Log ("Saving settings");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create(pathToSaveFile);
		bf.Serialize (file, settingsOfTheGame);
		file.Close ();
	}

	void LoadSettings(){
		Debug.Log ("Loading settings");
		if (File.Exists (pathToSaveFile)) {
			Debug.Log ("Loading settings from filename = " + pathToSaveFile);
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (pathToSaveFile, FileMode.Open);
			settingsOfTheGame = (CustomGameSettings)bf.Deserialize (file);
			file.Close ();
			Debug.Log ("settings loaded");
		} else {
			Debug.Log ("making new settings object");
			settingsOfTheGame = new CustomGameSettings ();
		}
	}
}

//NEEDS: - playlist un-check not to play, track by track
[Serializable]
public class CustomGameSettings{
	public float mainVolume;
	public float musicVolume;
	public float fxVolume;
	public float khVolume;
	public float explosionsVolume;
	public float buzzVolume;
	public float winningVolume;
	public float loosingVolume;

	public float[] mainMenuMusicVolumes;
	public float[] shooterMusicVolumes;

	public CustomGameSettings(){
		Debug.Log ("initialising new settings object");
		mainMenuMusicVolumes = new float[6];
		shooterMusicVolumes = new float[5];
	}
}