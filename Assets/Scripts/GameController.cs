using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject[] possible_enemy_types;
	public PlayerController player;
	public float killahertz;
	public Button game_over_menu;
	public GameObject game_won_panel;
	public GameObject game_lost_panel;
	public GameObject score_board;
	public AudioSource winning_sound;
	public AudioSource losing_sound;
	public Slider health_slider;
	public Slider hertz_slider;
	public UpdateGameTimer big_timer;
	public AudioClip bg_music_clip;
	public AudioMixerSnapshot volumeDown;
	public AudioMixerSnapshot volumeUp;
	public FaceChange pilot_face;

	private int enemiesMax;
	private int total_monsters;
	private int escaped_monsters;
	private WaitForSeconds win_blowup = new WaitForSeconds (0.02f);
	private WaitForSeconds restart_wait = new WaitForSeconds (5.0f);
	private bool gameover;
	private bool gamewon;
	private bool destroying;
	private int starting_health;
	private AudioSource bg_music;
	private float hp_percentage;


	private class score_card{
		public int kills = 0;
		public int shots = 0;
		public int hits  = 0;
		public int total = 0;
		public override string ToString(){
			return kills+" kills, "+shots+" shots, "+hits+" hits, "+total+" total";
		}
		public string GetGrade(){
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
	private score_card score;


	void Awake () {
		total_monsters = escaped_monsters = 0;
		score = new score_card();
		enemiesMax = 1;
		gameover = false;
		gamewon = false;
		destroying = false;
		big_timer.enabled = true;
		starting_health = player.health;
		if (bg_music_clip) {
			bg_music = GetComponent<AudioSource> ();
			bg_music.clip = bg_music_clip;
			bg_music.Play ();
		}
	}


	void Start(){
		StartCoroutine (FadeUp (0.5f));
	}


	void Update () {

		//When the game is either won or lost, display restart button, and allow for user to restart by pressing 'R'
		if (game_over_menu.IsActive() && Input.GetKeyDown (KeyCode.R)) {
			game_over_menu.onClick.Invoke ();
		}


		//Player health bar and face changes
		hp_percentage = (float)player.health / starting_health;
		health_slider.value = hp_percentage;
		if(pilot_face) pilot_face.HealthToFace(hp_percentage);


		if (!gameover) {
		//Game is not over
			if (total_monsters < enemiesMax) {
			//Make sure to make new monsters as long as necessary
				Instantiate (possible_enemy_types [Random.Range (0, possible_enemy_types.Length)]);
				total_monsters++;	//monsters currently alive
				score.total++;		//monsters created since start
			}


//	This block was used in first version, but makes no sense
//			float KE = score.kills / (escaped_monsters + 1.0f);
//			float KS = score.hits / (score.shots + 1.0f);
//			float hertz = KS * KE;


			//Fire rate for the basic weapon is equal to the number of monsters killed
			//Fire rate decreases for each monster that passes all the way though
			float hertz = score.kills - escaped_monsters;
			player.SetFireRateHZ (hertz);						//Player weapon fires at this rate
			hertz_slider.value = (float)hertz / killahertz;		//Show progress to reach KHz

			if (hertz >= killahertz) {
			//KHz level reached
				gameover = true;
				gamewon = true;
				game_won_panel.gameObject.SetActive (true);
			}
				
			if (player.health <= 0) {
			//Player was killed
				health_slider.value = 0;
				gameover = true;
				gamewon = false;
				game_lost_panel.gameObject.SetActive (true);
			}


		} else {
		//Game is over, one way or another
			StartCoroutine (FadeDown (5f));
			big_timer.enabled = false;
			if (!destroying) {
				GameOver ();
				if (game_over_menu)
					StartCoroutine (ShowRestartButton());
			}
		}
	}


	void GameOver(){
		if (player) {
			if (gamewon) {
				if (winning_sound)
					winning_sound.Play ();
				StartCoroutine (DestroyAllEnemies ());
			} else if (losing_sound)
				losing_sound.Play ();
			Destroy (player.gameObject);
			score_board.SetActive(true);

			score_board.GetComponent<ScoreBoardController> ().ShowScores(score.GetGrade(), score.kills, score.total, score.shots, score.hits);
		}
	}


	private IEnumerator DestroyAllEnemies(){
	//blow up one at a time, wait 'win_blowup' seconds, blow up one, rpt...
		destroying = true;
		GameObject e = GameObject.FindWithTag ("Enemy");
		if (e) {
			e.GetComponent<EnemyAController> ().MakeLoud ();
			yield return win_blowup;
			Destroy (e);
			enemiesMax = total_monsters;
			StartCoroutine (DestroyAllEnemies ());
		} else {
			destroying = false;
		}
	}


	private IEnumerator ShowRestartButton(){
	//Enables the UI button for restarting after win/loss
		yield return restart_wait;
		game_over_menu.gameObject.SetActive(true);
	}


	public void MonsterDown(){
	//Signifies monster getting killed by player
		score.kills++;
		total_monsters--;
		if (!gameover) {
			enemiesMax++;
//			enemiesMax--;
		}
	}
	public void MonsterEscaped(){
	//Signifies monster not dying inside GameSpace, and escaping through the back
		escaped_monsters++;
	}
	public void ShotFired(){
	//Counts how many times the player fired their weapon
		score.shots++;
	}
	public void ShotHit(){
	//Monster was hit by player weapon
		score.hits++;
	}
	private IEnumerator FadeUp(float fadeTime){
	//"Slowly" increase volume until 'volumeUp' is reached
		volumeUp.TransitionTo (fadeTime);
		yield return null;
	}
	private IEnumerator FadeDown(float fadeTime){
	//"Slowly" decrease volume until 'volumeDown' is reached
		volumeDown.TransitionTo (fadeTime);
		yield return null;
	}
}
