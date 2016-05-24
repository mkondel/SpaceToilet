using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour {

	public float notes = 1.059463f;

	public void SetHops(int hops){
		GetComponent<AudioSource>().pitch *= Mathf.Pow(notes, hops);
	}
}
