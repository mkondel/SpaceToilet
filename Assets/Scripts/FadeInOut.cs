using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeInOut : MonoBehaviour {

	public float fadeSpeed = 0.8f;
	public bool disableAfterDone = false;

	private Image myImage;
	private bool subtractAlpha=false;
	private float resetAlpha = 0f;
	private float endAlpha = 0f;


	//uses any user set image from the component, fades alpha from 1 down to user set image color.
	//At the start the scene is full opaque user def image, then image "FADES-OUT", in the end the scene is fully visible.
	public void FadeOut(){
		endAlpha = resetAlpha;
		subtractAlpha = true;
		this.SetAlpha(1);
	}

	//Reverse of FadeOut, uses any user set image from the component, fades alpha from 0 to user set image color.
	//At the start the scene is the same, then user defined image slowly "FADES-IN", and covers the screen.
	public void FadeIn(){
		endAlpha = resetAlpha;
		subtractAlpha = false;
		this.SetAlpha(0);
	}

	public void ResetFader(){
		this.SetAlpha(resetAlpha);
	}

	void SetAlpha( float alpha ){
		myImage.color = new Color (myImage.color.r, myImage.color.g, myImage.color.b, Mathf.Clamp01 (alpha));
	}

	void Awake(){
		myImage = GetComponent<Image> ();
		resetAlpha = myImage.color.a;
		ResetFader ();
	}

	void Update(){
		if (this.gameObject.activeSelf) {
			if (myImage.color.a == Mathf.Clamp01 (endAlpha)) {
				Debug.Log ("done with fade " + myImage.color.a);
				if (disableAfterDone) {
					this.gameObject.SetActive (false);
				}
			} else {
				float newAlpha;
				if (subtractAlpha) {
					newAlpha = myImage.color.a - fadeSpeed * Time.deltaTime;
					if (myImage.color.a < resetAlpha) {
						newAlpha = resetAlpha;
					}
				} else {
					newAlpha = myImage.color.a + fadeSpeed * Time.deltaTime;
					if (myImage.color.a > resetAlpha) {
						newAlpha = resetAlpha;
					}
				}
				this.SetAlpha (newAlpha);
			}
		}
	}
}
