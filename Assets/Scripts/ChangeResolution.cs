using UnityEngine;
using System.Collections;

public class ChangeResolution : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Screen.SetResolution (500, 500, false);
		Debug.Log("hmm");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
