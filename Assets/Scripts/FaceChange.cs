using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FaceChange : MonoBehaviour {

	public Sprite[] normalFaces;
	public Sprite[] killaFaces;

	private Image face_image;
	private bool khzMode;
	public bool KhzMode {
		get {
			return khzMode;
		}
		set {
			khzMode = value;
		}
	}

	void Awake(){
		face_image = GetComponent<Image> ();
		face_image.color = Color.white;
	}

	public void HealthToFace(float hp){
		Sprite[] face_array;
		//check if in khz mode, and use negatives instead
		if (khzMode) {
			face_array = killaFaces;
		} else {
			face_array = normalFaces;
		}
		//hp is float 0-1 === percentage of health left
		if (face_image && face_array.Length > 0) {
			if (hp > 0) {
				face_image.sprite = face_array [Mathf.Clamp ((int)((face_array.Length - 1) * hp) + 1, 1, face_array.Length - 1)];
			} else {
				face_image.sprite = face_array[0];
				face_image.color = Color.red;
			}
		}
	}
}
