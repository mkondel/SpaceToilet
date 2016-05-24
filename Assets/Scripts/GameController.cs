using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject[] possibleEnemyTypes;
	public PlayerController player;
	public int killahertz;
	public Button gameOverMenu;
	public GameObject gameWonPanel;
	public GameObject gameLostPanel;
	public GameObject scoreBoard;
	public AudioSource winningSound;
	public AudioSource losingSound;
	public Slider healthSlider;
	public Slider hertzSlider;
	public UpdateGameTimer bigTimer;
	public AudioClip bgMusicClip;
	public AudioMixerSnapshot volumeDown;
	public AudioMixerSnapshot volumeUp;
	public FaceChange pilotFace;
	public GameObject pilotLightningBG;

	private int enemiesMax;
	private int totalMonsters;
	private int escapedMonsters;
	private static WaitForSeconds winBlowup = new WaitForSeconds (0.02f);
	private static WaitForSeconds restartButtonWait = new WaitForSeconds (5.0f);
	private bool gameover;
	private bool gamewon;
	private int startingHealth;
	private AudioSource bgMusic;
	private float hpPercent;
	private bool KHz;
	private int currHertz;


	private class ScoreCard{
		public int kills = 0;
		public int shots = 0;
		public int hits  = 0;
		public int total = 0;

		public override string ToString(){return kills+" kills, "+shots+" shots, "+hits+" hits, "+total+" total";}

		public string GetGrade(int s){
			shots = s;
			string possible_grades = "XXXXXFDCBA";
			float acc = (float)hits/shots;
			float kill_ratio = (float)kills/total;
			float grade_value = (acc + kill_ratio)/2.0f;
			int idx = (int)(grade_value * (float)possible_grades.Length);
			if (idx < 5) idx = 5;
			if (idx >= possible_grades.Length) idx = possible_grades.Length-1;
			return possible_grades[idx].ToString();
		}
	};
	private ScoreCard score;


	void Awake () {
		totalMonsters = escapedMonsters = 0;
		score = new ScoreCard();
		enemiesMax = 1;
		currHertz = 1;
		gameover = false;
		gamewon = false;
		KHz = false;
		player.KHz = killahertz;
		bigTimer.enabled = true;
		startingHealth = player.health;
		if (bgMusicClip) {
			bgMusic = GetComponent<AudioSource> ();
			bgMusic.clip = bgMusicClip;
			bgMusic.Play ();
		}
	}


	void Start(){
		StartCoroutine (FadeUp (0.5f));
//		pilotFace.KhzMode = true;
//		pilotLightningBG.SetActive (true);
	}


	void Update () {

		//When the game is either won or lost, display restart button, and allow for user to restart by pressing 'R'
		if (gameOverMenu.IsActive() && Input.GetKeyDown (KeyCode.R)) {
			gameOverMenu.onClick.Invoke ();
		}

		//Player health bar and face changes
		PlayerHealthAndFace();

		if (!gameover) {	//Game is not over
			if (totalMonsters < enemiesMax) {
				RandomlyPlacedNewMonster();		//Make sure to make new monsters as long as necessary
			}else if (totalMonsters == 0) {
				GameWon();
			}

			if (currHertz >= killahertz) {
				currHertz = killahertz;
				KHz = true;
				pilotLightningBG.SetActive (true);
				pilotFace.KhzMode = true;
			} else {
				KHz = false;
				pilotLightningBG.SetActive (false);
				pilotFace.KhzMode = false;
			}
				
			if (player.health <= 0) {
				PlayerDead ();
				KHz = false;
			}

			UpdateGunAndDisplay (currHertz);	//Set gun fire rate and Hz bar display

		} else {
		//Game is over, one way or another
			StartCoroutine (FadeDown (5f));
			bigTimer.enabled = false;
			GameOver ();
			if (gameOverMenu && !gameOverMenu.gameObject.activeInHierarchy)
				StartCoroutine (ShowRestartButton());
		}
	}


	void GameWon(){
	//Player wins!
		gameover = gamewon = true;
		gameWonPanel.gameObject.SetActive (true);
	}


	void RandomlyPlacedNewMonster(){
	//Spawn a new enemy object
		Instantiate (possibleEnemyTypes [Random.Range (0, possibleEnemyTypes.Length)]);
		totalMonsters++;	//monsters currently alive
		score.total++;		//monsters created since start
	}


	void UpdateGunAndDisplay(float hertz){
	//Fire rate for the basic weapon is equal to the number of monsters killed
	//Fire rate decreases for each monster that passes all the way though
		player.SetFireRateHZ (hertz);						//Player weapon fires at this rate
		hertzSlider.value = hertz / killahertz;		//Show progress to reach KHz
	}


	void PlayerHealthAndFace(){
	//Update to show current values of health in both the bar and pilot face
		hpPercent = (float)player.health / startingHealth;
		healthSlider.value = hpPercent;
		if (pilotFace)
			pilotFace.HealthToFace (hpPercent);
	}


	void PlayerDead(){
	//Player was killed
		healthSlider.value = 0;
		gameover = true;
		gamewon = false;
		gameLostPanel.gameObject.SetActive (true);
	}


	void GameOver(){
		if (player) {
			if (gamewon && winningSound && winningSound.isActiveAndEnabled)
				winningSound.Play ();
			else if (losingSound && losingSound.isActiveAndEnabled)
				losingSound.Play ();
			Destroy (player.gameObject);
//			pilotLightningBG.SetActive (false);
			scoreBoard.SetActive(true);

			scoreBoard.GetComponent<ScoreBoardController> ().ShowScores(score.GetGrade(player.Shots), score.kills, score.total, player.Shots, score.hits);
		}
	}


	private IEnumerator ShowRestartButton(){
	//Enables the UI button for restarting after win/loss
		yield return restartButtonWait;
		gameOverMenu.gameObject.SetActive(true);
	}


	public void MonsterKilled(){	//Signifies monster getting killed by player
		score.kills++;
		currHertz++;
		totalMonsters--;
		if (!gameover && !KHz) {
			Debug.Log ("Enemies: "+totalMonsters+" MAX: "+enemiesMax+" basic weapon");
			enemiesMax++;
		}else{
			Debug.Log ("Enemies: "+totalMonsters+" MAX: "+enemiesMax+" KHz mode");
			enemiesMax--;
		}
	}
	public void MonsterEscaped(){	//Signifies monster not dying inside GameSpace, and escaping through the back
		escapedMonsters++;
		if (currHertz>0) {
			currHertz--;
		}
		totalMonsters--;
	}
	public void ShotHit(){			//Monster was hit by player weapon
		score.hits++;
	}
	private IEnumerator FadeUp(float fadeTime){		//"Slowly" increase volume until 'volumeUp' is reached
		volumeUp.TransitionTo (fadeTime);
		yield return null;
	}
	private IEnumerator FadeDown(float fadeTime){	//"Slowly" decrease volume until 'volumeDown' is reached
		volumeDown.TransitionTo (fadeTime);
		yield return null;
	}
}
