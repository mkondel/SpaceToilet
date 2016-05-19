using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

public class GameController : MonoBehaviour {
	
	public GameObject[] possible_enemy_types;
	public PlayerController player;
	public float megahertz;
	public Button game_over_menu;
	public Text game_won_text;
	public GameObject game_lost_text;
	public AudioSource winning_sound;
	public AudioSource losing_sound;
	public Slider health_slider;
	public Slider hertz_slider;
	public UpdateGameTimer big_timer;
	public AudioClip bg_music_clip;
	public AudioMixerSnapshot volumeDown;
	public AudioMixerSnapshot volumeUp;

	private int enemiesMax;
	private int total_monsters;
	private int killed_monsters;
	private int escaped_monsters;
	private int shots_fired;
	private int shots_hit;
	private WaitForSeconds win_blowup = new WaitForSeconds (0.02f);
	private WaitForSeconds restart_wait = new WaitForSeconds (5.0f);
	private bool gameover;
	private bool gamewon;
	private bool destroying;
	private int starting_health;
	private AudioSource bg_music;

	void Awake () {
		total_monsters = killed_monsters = shots_fired = escaped_monsters = 0;
		enemiesMax = 100;
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
		
	private IEnumerator FadeUp(float fadeTime){
		volumeUp.TransitionTo (fadeTime);
		yield return null;
	}

	private IEnumerator FadeDown(float fadeTime){
		volumeDown.TransitionTo (fadeTime);
		yield return null;
	}

	void Start(){
		StartCoroutine (FadeUp (0.5f));
	}

	void Update () {
		if (game_over_menu.IsActive() && Input.GetKeyDown (KeyCode.R)) {
			game_over_menu.onClick.Invoke ();
		}

		health_slider.value = (float)player.health/starting_health;

		if (!gameover) {
			if (total_monsters < enemiesMax) {
				Instantiate (possible_enemy_types [Random.Range (0, possible_enemy_types.Length)]);
				total_monsters++;
			}

			float KE = killed_monsters / (escaped_monsters + 1.0f);
			float KS = shots_hit / (shots_fired + 1.0f);
			float hertz = KS * KE;
			player.SetFireRateHZ (hertz);

			hertz_slider.value = (float)hertz / megahertz;

			if (hertz > megahertz) {
				gameover = true;
				gamewon = true;
				game_won_text.gameObject.SetActive (true);
			}

			if (player.health < 0) {
				health_slider.value = 0;
				gameover = true;
				gamewon = false;
				game_lost_text.gameObject.SetActive (true);
			}
		} else {
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
		}
	}

	private IEnumerator DestroyAllEnemies(){
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
		yield return restart_wait;
		game_over_menu.gameObject.SetActive(true);
	}

	public void MonsterDown(){
		killed_monsters++;
		total_monsters--;
		if (!gameover) {
			enemiesMax++;
		}
	}

	public void MonsterEscaped(){
		escaped_monsters++;
	}

	public void ShotFired(){
		shots_fired++;
	}

	public void ShotHit(){
		shots_hit++;
	}
}
