using UnityEngine;
using System.Collections;

public class ScrollTexture : MonoBehaviour {

	public float scrollSpeed = 0.005f;
	public float startAcceleration = 0.01f;
	public Renderer rend;
	private float currSpeed;

	void Start() {
		rend = GetComponent<Renderer>();
//		rend.material.mainTextureOffset = new Vector2(rend.material.mainTextureOffset.x, Random.value);
		currSpeed = 0.0f;
	}

	void Update() {
		if (currSpeed < scrollSpeed) {
			currSpeed += scrollSpeed * startAcceleration;
//			Debug.Log ("currSpeed is " + currSpeed);
		}
		float offset = rend.material.mainTextureOffset.y + currSpeed * Time.deltaTime;
		rend.material.mainTextureOffset = new Vector2(rend.material.mainTextureOffset.x, offset);
	}
}
