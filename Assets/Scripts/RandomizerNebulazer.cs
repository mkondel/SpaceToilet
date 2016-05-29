using UnityEngine;
using System.Collections;

public class RandomizerNebulazer : MonoBehaviour {

	public Sprite[] possible_sprites;
	public Vector3 speed = Vector3.zero;
	public Vector3 rotate = Vector3.zero;
	public float secBetweenSprites = 1;
	public Vector3 posVariation = Vector3.zero;
	public Vector3 rotVariation = Vector3.zero;
	public Vector3 speedVariation = Vector3.zero;
	public GameObject nebulaPrefab;

	private float nextTime = 0;
	
	// Update is called once per frame
	void Update () {
		if (nextTime < Time.time) {
			GetRandomNebula ();
			nextTime = Time.time + secBetweenSprites;
		}
	}

	void GetRandomNebula (){
		GameObject me = Instantiate (nebulaPrefab, GetRandomFromRange(transform.position, posVariation), nebulaPrefab.transform.rotation) as GameObject;

		me.GetComponent<SpriteRenderer> ().sprite = possible_sprites[Random.Range(0, possible_sprites.Length)];
		me.GetComponent<SpriteRenderer> ().flipX = Random.Range (0, 2) > 0;
		me.GetComponent<SpriteRenderer> ().flipY = Random.Range (0, 2) > 0;

		me.GetComponent<SimpleMover>().speed = GetRandomFromRange (speed, speedVariation);
		me.GetComponent<SimpleMover>().rotate = GetRandomFromRange (rotate, rotVariation);
	}


	//treats vec2 input myRange as the range to get random values from
	Vector3 GetRandomFromRange (Vector3 myVar, Vector3 myRange, int xMult=1, int yMult=1, int zMult=1){
		Vector3 randomSpeed = new Vector3(Random.Range(-myRange.x, myRange.x)*xMult, Random.Range(-myRange.y, myRange.y)*yMult, Random.Range(-myRange.z, myRange.z)*zMult);
		return myVar + randomSpeed;
	}
}
