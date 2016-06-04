using UnityEngine;
using System.Collections;

public class ImagePickerByOS : MonoBehaviour {

	public Vector3 iosDeltaPos;
	public float iosDeltaScale;
	public Vector3 droidDeltaPos;
	public float droidDeltaScale;

	// Use this for initialization
	void Start () {
		
		RectTransform rect = GetComponent<RectTransform>();

		#if UNITY_IOS
		rect.localScale *= iosDeltaScale;
		rect.position += iosDeltaPos;

		#elif UNITY_ANDROID
		rect.localScale *= droidDeltaScale;
		rect.position += droidDeltaPos;

		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
