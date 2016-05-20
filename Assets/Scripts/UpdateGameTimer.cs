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
		float time_float = Time.timeSinceLevelLoad;
		int seconds_only = Mathf.RoundToInt (time_float);
		int secs = seconds_only % 60;
		int mins = seconds_only / 60;
		int ms   = ((int)(time_float*1000f) - seconds_only)%1000;
		my_text.text = string.Format("{0:00}:{1:00}:{2:000}ms", mins, secs, ms);
	}
}
