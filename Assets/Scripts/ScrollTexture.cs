using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {

	public float scrollSpeed = 0.005f;
	public float startAcceleration = 0.01f;
	private Renderer rend;
	private float currSpeed;
	private Vector2 texOff;

	void Start() {
		rend = GetComponent<Renderer>();
//		rend.material.mainTextureOffset = new Vector2(rend.material.mainTextureOffset.x, Random.value);
		currSpeed = 0.0f;
		texOff = new Vector2 (rend.material.mainTextureOffset.x, rend.material.mainTextureOffset.y);
	}

	void Update() {
		if (currSpeed < scrollSpeed) {
			currSpeed += scrollSpeed * startAcceleration;
//			Debug.Log ("currSpeed is " + currSpeed);
		}
		texOff.y = rend.material.mainTextureOffset.y + currSpeed * Time.deltaTime;
		rend.material.mainTextureOffset = texOff;
	}
}
