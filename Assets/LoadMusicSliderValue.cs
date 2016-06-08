using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class LoadMusicSliderValue : MonoBehaviour {

	public Persister pers;
	public enum VolTypes {MAIN, MUSIC, FX, KHZ, EXPLOSION, BUZZ, WINNING, LOOSING};
	public VolTypes kindOfVolume;

	// Use this for initialization
	void Start(){
		Slider mySlider = GetComponent<Slider> ();


		float volVal = 0f;
		switch (kindOfVolume) {
		case VolTypes.MAIN:
			volVal = pers.settingsOfTheGame.mainVolume;
			break;
		case VolTypes.MUSIC:
			volVal = pers.settingsOfTheGame.musicVolume;
			break;
		case VolTypes.FX:
			volVal = pers.settingsOfTheGame.fxVolume;
			break;
		case VolTypes.KHZ:
			volVal = pers.settingsOfTheGame.khVolume;
			break;
		case VolTypes.EXPLOSION:
			volVal = pers.settingsOfTheGame.explosionsVolume;
			break;
		case VolTypes.BUZZ:
			volVal = pers.settingsOfTheGame.buzzVolume;
			break;
		case VolTypes.WINNING:
			volVal = pers.settingsOfTheGame.winningVolume;
			break;
		case VolTypes.LOOSING:
			volVal = pers.settingsOfTheGame.loosingVolume;
			break;
		default:
			break;
		}

		Debug.Log ("Start in LoadMusicSliderValue " + volVal);
		mySlider.value = Mathf.Clamp( volVal, mySlider.minValue, mySlider.maxValue );
	}
}
