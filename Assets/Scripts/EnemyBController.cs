using UnityEngine;
using System.Collections;

public class EnemyBController : EnemyController {

	// Use this for initialization
	void Start () {
		GetGameController ();
		GetOneAudioSource ();
		original_speed = speed;
		original_scale = myBodyObject.transform.localScale;
		original_radius = GetComponent<SphereCollider> ().radius;
		Reset ();
	}

	void Reset(){
		speed = original_speed;
		myBodyObject.transform.localScale = original_scale;
		GetComponent<SphereCollider> ().radius = original_radius;
		hops = 1;
		hp = 1;
		in_game = false;
		PickNewStartingPoint ();
		//		while (hops < hop_limit) Shrink ();
	}

	void PickNewStartingPoint(){
		transform.position = startingPoint + new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), Random.Range (zMin, zMax));
	}

	void OnTriggerEnter(Collider other) {
		if (in_game) {
			if (other.tag == "Player") {
				hp = 0;
				other.GetComponent<PlayerController> ().TakeDamage (hops);
			} else if (other.tag == "Bullet") {
				GC.ShotHit ();
			}
		} else { //not in game space (yet?)
			if (other.tag == "GameSpace") {
				in_game = true;
			}
		}
	}

	void OnTriggerStay(Collider other) {
		if (other.tag == "Enemy") {
			if (!in_game) {
				if (hops < hop_limit) {
					Shrink ();
				} else {
					//hop_limit reaches
					startingPoint += move_back_vector;
					zMin += move_back_vector.z;
					zMax += move_back_vector.z;
					Reset ();
				}
			}
		}
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "GameSpace") {
			in_game = false;
		}
	}

	void OnDestroy(){
		if (in_game) {
			GC.MonsterKilled ();
			MakeDeathSound ();
			Explode ();
		}
		else
			GC.MonsterEscaped ();
	}

	void GetGameController(){
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameSpace");
		if (gameControllerObject != null){
			GC = gameControllerObject.GetComponent <GameController>();
		}
		if (GC == null){
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void GetOneAudioSource(){
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("EnemyAudioSource");
		if (gameControllerObject != null){
			oneEnemySoundSource = gameControllerObject.GetComponent <AudioSource>();
		}
		if (oneEnemySoundSource == null){
			Debug.Log ("Cannot find oneEnemySoundSource object with tag EnemyAudioSource");
		}
	}
}
