﻿using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Audio;

//Persister class keeps volume/mouse settings between scenes.
//Also saves them to a binary file to persist between app restarts.
//Persister responds to ESC key to stop animation and opening theme.
public class Persister : MonoBehaviour {

	public static Persister persister;
	public string dataFileName = "savedSettings.data";
	public CustomGameSettings settingsOfTheGame;
	public AudioMixer mainMixer;
	public GameObject pause_menu;
	public GameTitle gameTitle;

	private string pathToSaveFile;
	private bool isPaused;

	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Cancel") && !isPaused){
			DoPause();
		} else if (Input.GetButtonDown ("Cancel") && isPaused){
			UnPause ();
		}
	}

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

	void Start(){
		SetSettings();
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

	void SetSettings(){
		setMainVolume( settingsOfTheGame.mainVolume );
		setMusicVolume( settingsOfTheGame.musicVolume );
		setFxVolume( settingsOfTheGame.fxVolume );
	}

	public void setMainVolume(float newLvl){
		settingsOfTheGame.mainVolume = newLvl;
		mainMixer.SetFloat ("mainVol", newLvl);
	}
	public void setMusicVolume(float newLvl){
		settingsOfTheGame.musicVolume = newLvl;
		mainMixer.SetFloat("musicVol", newLvl);
	}
	public void setFxVolume(float newLvl){
		settingsOfTheGame.fxVolume = newLvl;
		mainMixer.SetFloat("sfxVol", newLvl);
	}

	public void DoPause(){
		isPaused = true;
		Time.timeScale = 0;		//Set time.timescale to 0, this will cause animations and physics to stop updating
		if (!gameTitle.Abort) {
			gameTitle.AbortEverything ();
			UnPause ();
		} else {
			pause_menu.SetActive (true);
		}
	}
	public void UnPause(){
		isPaused = false;
		Time.timeScale = 1;		//Set time.timescale to 1, animations and physics continue updating at regular speed
		pause_menu.SetActive (false);
	}
}

//NEEDS: - playlist un-check not to play, track by track
[Serializable]
public class CustomGameSettings{
	public float mainVolume;
	public float musicVolume;
	public float fxVolume;

	public float[] mainMenuMusicVolumes;
	public float[] shooterMusicVolumes;

	public CustomGameSettings(){
		Debug.Log ("initialising new CustomGameSettings object");
		mainMenuMusicVolumes = new float[6];
		shooterMusicVolumes = new float[5];
	}
}