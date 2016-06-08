using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadMusicSliderValue : MonoBehaviour {

	public Persister pers;
	public Vector2 limits = new Vector2(-40f,-0.001f);

	// Use this for initialization
	void Start(){
		Debug.Log ("Start in LoadMusicSliderValue " + pers.settingsOfTheGame.musicVolume);
		GetComponent<Slider>().value = Mathf.Clamp( pers.settingsOfTheGame.musicVolume, limits.x, limits.y );
	}
}
