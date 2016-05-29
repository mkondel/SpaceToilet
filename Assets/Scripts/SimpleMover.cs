using UnityEngine;
using System.Collections;

public class SimpleMover : MonoBehaviour {

	public Vector3 speed;
	public Vector3 rotate;
	
	// Update is called once per frame
	void Update () {
		transform.position += speed * Time.deltaTime;
		transform.rotation *= Quaternion.Euler (rotate * Time.deltaTime);
	}
}
