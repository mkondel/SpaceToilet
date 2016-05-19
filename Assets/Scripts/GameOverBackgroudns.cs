using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameOverBackgroudns : MonoBehaviour {

	public Sprite[] possible_backgrounds_lost;

	public float fadeSpeed = 0.3f;

	private Image my_image;
	private float threshhold = 0.5f;


	// Use this for initialization
	void Start () {
		my_image = GetComponent<Image> ();
		my_image.sprite = possible_backgrounds_lost [Random.Range (0, possible_backgrounds_lost.Length)];
		my_image.color = Color.clear;
		RectTransform my_panel = GetComponent<RectTransform> ();
		my_panel.localScale = RandomScale ();
	}

	void Update(){
		my_image.color = Color.Lerp (my_image.color, Color.white, fadeSpeed * Time.deltaTime);
	}

	private Vector3 RandomScale(){
		Vector3 rand_vec = new Vector3 (1, 1, 1);
		rand_vec.x = Random.value>threshhold ? -1 : 1;
		rand_vec.y = Random.value>threshhold ? -1 : 1;
		return rand_vec;
	}
}