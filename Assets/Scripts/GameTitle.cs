using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System.Collections;

//GameTitle class animates the letters of the title on game startup.
//Currently GameTitle class changes font size and line spacing.
//Starting with letters really oversized, then gradually shirinking and aligning.
public class GameTitle : MonoBehaviour {

	public Vector2 durations;
	public Vector2 font_line;
	public GameObject hat_spin;
	public GameObject menu_pane;
	public AudioSource musac;
	public AudioMixerSnapshot volumeUp;

	private Text text;
	private Vector2 deltas;
	private float curr_size;
	private bool activate_hat;
	private bool abort;
	private AudioSource introSong;

	public bool Abort {
		get {
			return abort;
		}
	}

	private Vector2 initial;

	// Use this for initialization
	void Awake () {
		text = GetComponent<Text> ();
		deltas = new Vector2 (font_line.x / durations.x, font_line.y / durations.y);
		initial.x = text.fontSize;
		initial.y = text.lineSpacing;
		introSong = GetComponent<AudioSource> ();
	}

	void Start(){
		volumeUp.TransitionTo (0);
		curr_size = initial.x;
		text.lineSpacing = initial.y;
		abort = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!abort) {
			introSong.enabled = true;
			musac.enabled = false;
			if (text.fontSize > font_line.x) {
				curr_size -= deltas.x * Time.deltaTime;
				text.fontSize = (int)curr_size;
			} else if (text.lineSpacing < font_line.y)
				text.lineSpacing += deltas.y * Time.deltaTime;
			else if(introSong && !introSong.isPlaying){
				AbortEverything ();
			}
		}
	}

	public void AbortEverything(){
		abort = true;
		text.fontSize = (int)font_line.x;
		text.lineSpacing = font_line.y;
		introSong.enabled = false;
		SpinHat (true);
	}

	private void SpinHat(bool spinning){
		Debug.Log ("SpinHat(" + spinning + ")");
		if (hat_spin)
			hat_spin.SetActive (spinning);
		if (menu_pane)
			menu_pane.SetActive (spinning);
		if (musac)
			musac.enabled = spinning;
	}
}
