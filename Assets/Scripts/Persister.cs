using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Xml;
using System.Xml.Serialization;
using System.Collections.Generic;

//Persister class keeps volume/mouse settings between scenes.
//Also saves them to a binary file to persist between app restarts.
//Persister responds to ESC key to stop animation and opening theme.
//Shows/hides options menu.  Menus are inside Persister prefab.
public class Persister : MonoBehaviour
{
	public static Persister persister;
	public string dataFileName = "savedSettings.data";
	public CustomGameSettings settingsOfTheGame;
	public AudioMixer mainMixer;
	public GameObject pauseMenu;
	public GameTitle gameTitle;
	public GameObject mainMenu;
	public GameObject fadeOutObject;
	public GameObject fadeInObject;
	public GameObject quitButtonInMenu;
	public Text mouseSensitivityText;
	public Slider mouseSensitivitySlider;
	public Slider mainVolSlider;
	public Slider musicVolSlider;
	public Slider effectsVolSlider;
	public GameObject[] controlsMenus;
	public GameObject controlsSettingsMenu;
	public GameObject helpMenu;
	public AudioClip[] bgMusicClipsShooter;
	public AudioClip[] bgMusicClipsMenu;
	public Dropdown difficultyDropdown;

	private string pathToSaveFile;
	private bool isPaused;
	private static WaitForSeconds waitingTime = new WaitForSeconds (1.2f);
	private FadeInOut fadeOut;
	private FadeInOut fadeIn;
	private GyroToText myGyro;
	private AudioSource bgMusic;

	void Awake ()
	{
		Debug.Log ("Awake in Persister()");
		pathToSaveFile = Application.persistentDataPath + "/" + dataFileName;
		Debug.Log ("pathToSaveFile = " + pathToSaveFile);

		fadeOut = fadeOutObject.GetComponent<FadeInOut> ();
		fadeIn = fadeInObject.GetComponent<FadeInOut> ();
		myGyro = GetComponent<GyroToText> ();
		bgMusic = GetComponent<AudioSource> ();

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
		Debug.Log ("Start in Persister");
		SetSettings ();
		EnablePlatformSpecifics ();
		SceneManager.LoadScene (0);
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown ("Cancel") && !isPaused) {
			DoPause ();
		} else if (Input.GetButtonDown ("Cancel") && isPaused) {
			UnPause ();
		}
	}
		
	void OnApplicationQuit ()
	{
		Debug.Log ("Saving settings because app is quitting...");
		SaveSettings ();
	}

	void OnLevelWasLoaded (int lvl)
	{
		Debug.Log ("OnLevelWasLoaded in Persister");
		DoFadeIn ();

		//if in main menu, only show back button in options.  when in shooter scene, also show the quit button there.
		int idx1 = SceneManager.GetActiveScene ().buildIndex;
		//can prolly delete idx1 and just use lvl here....
		quitButtonInMenu.SetActive((idx1 == 1));

		if (lvl == 0) {	//this is the main menu scene
			mainMenu.SetActive (true);
			PlayMusicForScene(bgMusicClipsMenu);
		}else if (lvl == 1) {
			//find the gyro controller
			//set the values for LR tilt
			//play music track from this scene
			PlayMusicForScene(bgMusicClipsShooter);
		}
	}

	private void PlayMusicForScene(AudioClip[] bgMusicClips){
		//load clips from this scene
		if (bgMusicClips.Length>0) {
			bgMusic.clip = bgMusicClips[UnityEngine.Random.Range(0, bgMusicClips.Length)];
			bgMusic.Play ();
		}
	}

	void SaveSettings ()
	{
		Debug.Log ("SaveSettings in Persister");
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (pathToSaveFile);
		bf.Serialize (file, settingsOfTheGame);

//		var serializer = new XmlSerializer(typeof(CustomGameSettings));
//		serializer.Serialize(file, settingsOfTheGame);

		file.Close ();
	}

	void LoadSettings ()
	{
		Debug.Log ("Loading settings in Persister");
		if (File.Exists (pathToSaveFile)) {
			Debug.Log ("Loading settings from filename = " + pathToSaveFile);

			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (pathToSaveFile, FileMode.Open);
			settingsOfTheGame = (CustomGameSettings)bf.Deserialize (file);

//			Debug.Log ("making serializer");
//			var serializer = new XmlSerializer(typeof(CustomGameSettings));
//			Debug.Log ("done making serializer");
//			Debug.Log ("using serializer");
//			settingsOfTheGame = serializer.Deserialize(file) as CustomGameSettings;
			Debug.Log ("done using serializer");

			file.Close ();
			Debug.Log ("settings loaded");
		} else {
			Debug.Log ("making new settings object");
			settingsOfTheGame = new CustomGameSettings ();
			settingsOfTheGame.GenerateFakeTopTenScores ();
			Debug.Log ("done making new settings object");
		}
	}

	void SetSettings ()
	{
		SetMainVolume (settingsOfTheGame.mainVolume);
		SetMusicVolume (settingsOfTheGame.musicVolume);
		SetFxVolume (settingsOfTheGame.fxVolume);
		myGyro.L = settingsOfTheGame.fullLeftTiltVector.AsVector3();
		myGyro.R = settingsOfTheGame.fullRightTiltVector.AsVector3();
		SetMouseSensitivity (settingsOfTheGame.mouseSensitivity);
		SetDifficulty (settingsOfTheGame.difficultyMode);
	}

	public void SetMainVolume (float newLvl)
	{
		settingsOfTheGame.mainVolume = newLvl;
		mainMixer.SetFloat ("mainVol", newLvl);
		if (mainVolSlider.value != newLvl) {
			mainVolSlider.value = newLvl;
		}
	}

	public void SetMusicVolume (float newLvl)
	{
		settingsOfTheGame.musicVolume = newLvl;
		mainMixer.SetFloat ("musicVol", newLvl);
		if (musicVolSlider.value != newLvl) {
			musicVolSlider.value = newLvl;
		}
	}

	public void SetFxVolume (float newLvl)
	{
		settingsOfTheGame.fxVolume = newLvl;
		mainMixer.SetFloat ("sfxVol", newLvl);
		if (effectsVolSlider.value != newLvl) {
			effectsVolSlider.value = newLvl;
		}
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
		isPaused = false;							//aint paused no more1
		Time.timeScale = 1;							//Set time.timescale to 1, animations and physics continue updating at regular speed
		pauseMenu.SetActive (false);				//hide the pause menu for current scene
		controlsSettingsMenu.SetActive(false); 		//hide the controls setting menu as well
		helpMenu.SetActive(false); 					//hide the help menu
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
		Debug.Log ("Changing to "+sceneIndex+" Scene");
		SceneManager.LoadScene (sceneIndex);
	}

	public void SaveTiltSettings(){
		settingsOfTheGame.fullLeftTiltVector.SetFromVector3(myGyro.L);
		settingsOfTheGame.fullRightTiltVector.SetFromVector3(myGyro.R);
	}

	public void SetMouseSensitivity(float newS){
		settingsOfTheGame.mouseSensitivity = newS;
		mouseSensitivityText.text = "Mouse Sensitivity " + newS.ToString ();
		if (mouseSensitivitySlider.value != newS) {
			mouseSensitivitySlider.value = newS;
		}
	}

	void EnablePlatformSpecifics(){
		#if UNITY_STANDALONE || UNITY_EDITOR || UNITY_WEBGL
			controlsMenus[1].SetActive(true);
		#elif UNITY_ANDROID || UNITY_IOS
			controlsMenus[0].SetActive(true);
		#endif
	}

	public void SetDifficulty(Int32 newDiff){
		//SetDifficulty is called by the dropdown in options menu
		//and will reset player health accordingly on the next ShooterScene load
		switch (newDiff) {
		case 0:			//Easy mode
//			startingPlayerHealth = 10000;
			break;
		case 1:			//Normal
//			startingPlayerHealth = 1000;
			break;
		case 2:			//Hard
//			startingPlayerHealth = 100;
			break;
		default:		//Non-sensical
			Debug.Log ("SetDifficulty received a value not in [0,1,2]: " + newDiff.ToString ()+". Setting to 1=Normal");
			newDiff = 1;
			break;
		}
		Debug.Log ("SetDifficulty=" + newDiff.ToString ());//+" startingPlayerHealth="+startingPlayerHealth.ToString());
		difficultyDropdown.value = newDiff;				//set the menu if loading settings from file
		settingsOfTheGame.difficultyMode = newDiff;		//set the settingsOfTheGame value to be saved to file
	}
}


//SerializableVectorThree class helps save/load settings, unity does not serialize vec3 by itself
[Serializable]
[XmlRoot("CustomGameSettings")]
public class SerializableVectorThree
{
	private float x;
	public float X {
		get {
			return x;
		}
		set {
			x = value;
		}
	}

	private float y;
	public float Y {
		get {
			return y;
		}
		set {
			y = value;
		}
	}

	private float z;
	public float Z {
		get {
			return z;
		}
		set {
			z = value;
		}
	}

	public Vector3 AsVector3 (){
		return new Vector3 (x, y, z);
	}

	public void SetFromVector3(Vector3 v){
		X = v.x;
		Y = v.y;
		Z = v.z;
	}
}

//NEEDS: - playlist un-check not to play, track by track
[Serializable]
[XmlRoot("CustomGameSettings")]
//[System.Serializable]
public class CustomGameSettings
{
	public float mainVolume;
	public float musicVolume;
	public float fxVolume;
	public float[] mainMenuMusicVolumes;
	public float[] shooterMusicVolumes;
	public SerializableVectorThree fullLeftTiltVector;
	public SerializableVectorThree fullRightTiltVector;
	public float mouseSensitivity;
	public List<OneScoreFromTopTen> topTenScores;
	public Int32 difficultyMode;
	public string[] fakeNamesList = new string[10]{"John","Anastasia","Kirk","Mary","Fran","Bob","Ahmed","Ted","Ned","Zed"};

	public CustomGameSettings ()
	{
		Debug.Log ("initialising new CustomGameSettings object");

		//these are good starting defaults for tilt on my phone...
		fullLeftTiltVector = new SerializableVectorThree();
		fullRightTiltVector = new SerializableVectorThree();
		fullLeftTiltVector.SetFromVector3( new Vector3(-1f, 0f, -1f) );
		fullRightTiltVector.SetFromVector3( new Vector3(1f, 0f, -1f) );

		//set default difficulty to normal = 1 (easy = 0, hard = 2)
		difficultyMode = 1;
	}

	public void GenerateFakeTopTenScores(){
		//generate fake top 10 scores
		topTenScores = new List<OneScoreFromTopTen>();
		for (int i = 0; i < 10; i++) {
			OneScoreFromTopTen newScore = new OneScoreFromTopTen ();
			newScore.MakeRandomScore();
			//append top 10 rank to the name for debugging
			newScore.PlayerName = fakeNamesList[i];
			topTenScores.Add(newScore);
			Debug.Log("Made this in Persister: "+topTenScores[i].ToString());
		}
		topTenScores.Sort ();
	}

	//CheckIfInTopTen returns true if newScore is higher than the current 10th place
	public bool CheckIfInTopTen(OneScoreFromTopTen newScore){
		//if new score is higher than 10th score
		//CompareTo() returns <0 if this precedes other in the sort order
		if (newScore.CompareTo (topTenScores [topTenScores.Count-1]) < 0) {
			return true;
		} else {
			return false;
		}
	}

	//InsertNewTopTenScore adds the new score to the top ten list and sorts the new list, makes sure to keep only the top 10
	public void InsertNewTopTenScore(OneScoreFromTopTen newScore){
		//make a temp list to hold 11 scores
		List<OneScoreFromTopTen> tempTopElevenScores = new List<OneScoreFromTopTen>( topTenScores.ToArray());

		//add to the score list (count will be 11)
		tempTopElevenScores.Add (newScore);

		//sort
		tempTopElevenScores.Sort();

		//only keep the top 10 (count will be 10)
		tempTopElevenScores.RemoveAt(tempTopElevenScores.Count-1);

		//real top scores = new top 10 scores
		topTenScores = tempTopElevenScores;
	}
}


//this class handles one score from top 10
[Serializable]
[XmlRoot("CustomGameSettings")]
//[System.Serializable]
public class OneScoreFromTopTen : IComparable{
	private string playerName;

	public string PlayerName {
		get {
			return playerName;
		}
		set {
			playerName = value;
		}
	}

	private float timeValue;

	public float TimeValue {
		get {
			return timeValue;
		}
		set {
			timeValue = value;
		}
	}
	public string TimeValueAsString(){
		return UpdateGameTimer.TurnIntMillisecondsToString (timeValue);
//		return timeValue.ToString ();
	}

	private float accuracyValue;

	public float AccuracyValue {
		get {
			return accuracyValue;
		}
		set {
			accuracyValue = value;
		}
	}
	public string AccuracyValueAsString(){
		return accuracyValue.ToString ("P0");
	}

	private float killsValue;

	public float KillsValue {
		get {
			return killsValue;
		}
		set {
			killsValue = value;
		}
	}
	public string KillsValueAsString(){
		return killsValue.ToString ("P0");
	}

	private int difficulty;
	public int Difficulty {
		get {
			return difficulty;
		}
		set {
			difficulty = value;
		}
	}

	public OneScoreFromTopTen(){
		//constructor
	}

	public OneScoreFromTopTen MakeRandomScore(){
		PlayerName = "Fake Name ";
		TimeValue = UnityEngine.Random.Range(10000,50000)/1000f;
		AccuracyValue = UnityEngine.Random.Range (30, 99)/100f;
		KillsValue = UnityEngine.Random.Range (30, 99)/100f;
		Difficulty = Mathf.FloorToInt(UnityEngine.Random.Range(0,3));
		Debug.Log ("Made fake score "+this.ToString ());
		return this;
	}

	public string GradeMe(){
		string possible_grades = "FFFFFFDCBA";
		float grade_value = (accuracyValue + killsValue)/2.0f;
		int idx = (int)(grade_value * (float)possible_grades.Length);
		if (idx >= possible_grades.Length) idx = possible_grades.Length-1;
		return possible_grades[idx].ToString() +new string[]{" (e)"," (n)"," (h)"}[Difficulty];
	}

	public float GradeMeAsFloat(){
		return (accuracyValue + killsValue)/2.0f;
	}

	public override string ToString ()
	{
		return string.Format ("[OneScoreFromTopTen: PlayerName={0}, TimeValue={1}, Grade={2}, AccuracyValue={3}, KillsValue={4}]", PlayerName, TimeValue, GradeMe(), AccuracyValue, KillsValue);
	}

	public int CompareTo(object obj){
		if (obj == null) return -1;

		OneScoreFromTopTen other = obj as OneScoreFromTopTen;

		if (this.GradeMeAsFloat() > other.GradeMeAsFloat()) {
			//this grade is higher, automatically makes this precede other in the top ten list
			return -1;
		}else if(this.GradeMeAsFloat() == other.GradeMeAsFloat()){
			//grades are the same for this and other
			//if other time is shorter, then other is higher on the top 10 list, this follows other in the top ten list
			if (other.TimeValue < this.TimeValue) {
				return 1;
			}else{
				return 0;
			}
		}

		//same by default?
		return 0;
	}
}
