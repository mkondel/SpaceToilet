using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

//Persister class keeps volume/mouse settings between scenes.
//Also saves them to a binary file to persist between app restarts.
//Persister responds to ESC key to stop animation and opening theme.
//Shows/hides pause menu, which is different in each scene.  Both current pause menus are inside Persister prefab.
public class Persister : MonoBehaviour
{

	public static Persister persister;
	public string dataFileName = "savedSettings.data";
	public CustomGameSettings settingsOfTheGame;
	public AudioMixer mainMixer;
	public GameObject[] pause_menu_by_scene;
	public GameTitle gameTitle;
	public GameObject mainMenu;
	public GameObject fadeOutObject;
	public GameObject fadeInObject;
	public GameObject shooterMenu;

	private string pathToSaveFile;
	private bool isPaused;
	private GameObject pauseMenu;
	private static WaitForSeconds waitingTime = new WaitForSeconds (1.2f);
	private FadeInOut fadeOut;
	private FadeInOut fadeIn;

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Cancel") && !isPaused) {
			DoPause ();
		} else if (Input.GetButtonDown ("Cancel") && isPaused) {
			UnPause ();
		}
	}

	void Awake ()
	{
		Debug.Log ("Awake in Persister()");
		pathToSaveFile = Application.persistentDataPath + "/" + dataFileName;
		Debug.Log ("pathToSaveFile = " + pathToSaveFile);

		SetSettings ();
		pauseMenu = pause_menu_by_scene [SceneManager.GetActiveScene ().buildIndex];
		fadeOut = fadeOutObject.GetComponent<FadeInOut> ();
		fadeIn = fadeInObject.GetComponent<FadeInOut> ();

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

	void OnApplicationQuit ()
	{
		Debug.Log ("Saving settings because app is quitting...");
		SaveSettings ();
	}

	void OnLevelWasLoaded (int lvl)
	{
		DoFadeIn ();
		pauseMenu = pause_menu_by_scene [SceneManager.GetActiveScene ().buildIndex];
		if (lvl == 0) {	//this is the main menu scene
			mainMenu.SetActive (true);
		}else if (lvl == 1) {
			shooterMenu.SetActive (true);
		}
	}

	void SaveSettings ()
	{
		Debug.Log ("Saving settings");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (pathToSaveFile);
		bf.Serialize (file, settingsOfTheGame);
		file.Close ();
	}

	void LoadSettings ()
	{
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

	void SetSettings ()
	{
		setMainVolume (settingsOfTheGame.mainVolume);
		setMusicVolume (settingsOfTheGame.musicVolume);
		setFxVolume (settingsOfTheGame.fxVolume);
	}

	public void setMainVolume (float newLvl)
	{
		settingsOfTheGame.mainVolume = newLvl;
		mainMixer.SetFloat ("mainVol", newLvl);
	}

	public void setMusicVolume (float newLvl)
	{
		settingsOfTheGame.musicVolume = newLvl;
		mainMixer.SetFloat ("musicVol", newLvl);
	}

	public void setFxVolume (float newLvl)
	{
		settingsOfTheGame.fxVolume = newLvl;
		mainMixer.SetFloat ("sfxVol", newLvl);
	}

	public void DoPause ()
	{
		isPaused = true;
		Time.timeScale = 0;					//Set time.timescale to 0, this will cause animations and physics to stop updating
		if (!gameTitle.Abort) {				//if game title animation is playing
			gameTitle.AbortEverything ();	//stop playing the animation
			UnPause ();						//unpause to let main menu work
		} else {							//if no title animation is playing
			pauseMenu.SetActive (true);		//just show the pause menu for current scene
		}
	}

	public void UnPause ()
	{
		isPaused = false;
		Time.timeScale = 1;					//Set time.timescale to 1, animations and physics continue updating at regular speed
		pauseMenu.SetActive (false);		//hide the pause menu for current scene
	}

	public void BackToMenu ()
	{
		DoFadeOut ();
		StartCoroutine (LoadSceneDelayed (0));
		Time.timeScale = 1;		//unpause
	}

	public void StartShooter ()
	{
		DoFadeOut ();
		StartCoroutine( LoadSceneDelayed(1) ) ;
	}

	void DoFadeOut(){
		fadeOutObject.SetActive(true);
		fadeOut.FadeIn ();
	}

	void DoFadeIn(){
		fadeInObject.SetActive(true);
		fadeIn.FadeOut ();
	}

	public void RestartShooter ()
	{
		if (SceneManager.sceneCount < 1) {
			Debug.Log ("Cant load the scene needed to restart shooter...");
		} else {
			Debug.Log ("reloading shooter scene normally.");
			StartShooter ();
		}
	}

	IEnumerator LoadSceneDelayed (int sceneIndex)
	{
		yield return waitingTime;
		Debug.Log ("Changing to Shooter Scene");
		SceneManager.LoadScene (sceneIndex);
	}
}

//NEEDS: - playlist un-check not to play, track by track
[Serializable]
public class CustomGameSettings
{
	public float mainVolume;
	public float musicVolume;
	public float fxVolume;

	public float[] mainMenuMusicVolumes;
	public float[] shooterMusicVolumes;

	public CustomGameSettings ()
	{
		Debug.Log ("initialising new CustomGameSettings object");
		mainMenuMusicVolumes = new float[6];
		shooterMusicVolumes = new float[5];
	}
}