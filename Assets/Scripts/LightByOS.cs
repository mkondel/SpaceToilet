using UnityEngine;
using System.Collections;

public class LightByOS : MonoBehaviour {

	public GameObject regularLightsPrefab;
	public GameObject androidLightsPrefab;

	// Use this for initialization
	void Start () {
		#if UNITY_EDITOR
			Instantiate(regularLightsPrefab);
		#elif UNITY_ANDROID
			Instantiate(androidLightsPrefab);
		#else
			Instantiate(regularLightsPrefab);
		#endif
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
