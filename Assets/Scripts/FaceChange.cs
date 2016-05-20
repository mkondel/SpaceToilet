using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FaceChange : MonoBehaviour {

	public Sprite[] all_faces;

	private Image face_image;

	void Awake(){
		face_image = GetComponent<Image> ();
		face_image.color = Color.white;
	}

	public void HealthToFace(float hp){
		//hp is float 0-1 === percentage of health left
		if (face_image && all_faces.Length > 0) {
			if (hp > 0) {
				face_image.sprite = all_faces [Mathf.Clamp ((int)((all_faces.Length - 1) * hp) + 1, 1, all_faces.Length - 1)];
			} else {
				face_image.sprite = all_faces[0];
				face_image.color = Color.red;
			}
		}
	}
}
