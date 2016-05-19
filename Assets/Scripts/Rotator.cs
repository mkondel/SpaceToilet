using UnityEngine;
using System.Collections;

public class Rotator : MonoBehaviour {

	public Vector3 rotations;

	void Start(){
		rotations.x = 0;
		rotations.y = 0;
//		Debug.Log(rotations);
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation *= Quaternion.Euler (rotations);
	}
}
