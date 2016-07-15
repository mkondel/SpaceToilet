using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Fader : MonoBehaviour {

	public float fadeSpeed = 0.8f;
	public float startAlpha = 1;
	public float endAlpha = 0;
	public bool disableAfterDone = false;

	private Image myImage;

	void Start(){
		myImage = GetComponent<Image> ();
		myImage.color = new Color (myImage.color.r, myImage.color.g, myImage.color.b, Mathf.Clamp01 (startAlpha));
	}

	void Update(){
		if (myImage.color.a == Mathf.Clamp01 (endAlpha)) {
			Debug.Log ("done with fade "+myImage.color.a);
			if (disableAfterDone) {
				this.gameObject.SetActive (false);
			}
		} else {
			float newAlpha;
			if (startAlpha > endAlpha) {
				newAlpha = myImage.color.a - fadeSpeed * Time.deltaTime;
			} else {
				newAlpha = myImage.color.a + fadeSpeed * Time.deltaTime;
			}
//			Debug.Log (myImage.color.a+" fading to "+endColor.a);
			myImage.color = new Color (myImage.color.r, myImage.color.g, myImage.color.b, Mathf.Clamp01 (newAlpha)); //Color.Lerp (myImage.color, endColor, fadeSpeed);
		}
	}
}
