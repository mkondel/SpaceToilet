using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadMusicSliderValue : MonoBehaviour {

	public Persister pers;

	// Use this for initialization
	void Start(){
		Slider mySlider = GetComponent<Slider> ();
		Debug.Log ("Start in LoadMusicSliderValue " + pers.settingsOfTheGame.musicVolume);
		mySlider.value = Mathf.Clamp( pers.settingsOfTheGame.musicVolume, mySlider.minValue, mySlider.maxValue );
	}
}
