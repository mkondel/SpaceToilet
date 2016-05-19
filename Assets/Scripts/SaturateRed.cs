using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SaturateRed : MonoBehaviour {

	public Color goal;
	public Image tint;
	public float speed;
	
	// Update is called once per frame
	void Update () {
		tint.color = Color.Lerp (tint.color, goal, Time.deltaTime*speed);
	}
}
