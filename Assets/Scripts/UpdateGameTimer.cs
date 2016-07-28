using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UpdateGameTimer : MonoBehaviour {

	private Text my_text;

	// Use this for initialization
	void Awake () {
		my_text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		my_text.text = TurnIntMillisecondsToString (Time.timeSinceLevelLoad);
	}

	public static string TurnIntMillisecondsToString (float time_float){
		int seconds_only = Mathf.RoundToInt (time_float);
		int secs = seconds_only % 60;
		int mins = seconds_only / 60;
		int ms   = ((int)(time_float*1000f) - seconds_only)%1000;

		return string.Format("{0:00}:{1:00}:{2:000}ms", mins, secs, ms);
	}
}
