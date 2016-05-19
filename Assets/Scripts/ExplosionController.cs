using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	public float notes;

	public void SetHops(int hops){
		GetComponent<AudioSource>().pitch *= Mathf.Pow(notes, hops);
	}
}
