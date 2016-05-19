using UnityEngine;
using System.Collections;

public class EnemyAController : MonoBehaviour {

	public Vector3 speed;
	public float xMin, xMax, yMin, yMax, zMin, zMax;
	public Vector3 startingPoint;
	public GameObject explosion;
	public GameObject explosion_chink;
	public GameObject[] explosionSprite;
	public int hop_limit;
	public GameObject me;
	public float scale_factor;
	public int hp_max;

	private GameController GC;
	private bool loudDeath;
	private bool in_game;
	private int hops;
	private int hp;
	private GameObject boom;

	// Use this for initialization
	void Start () {
		loudDeath = true;
		boom = explosion;
		in_game = false;
		hops = 1;
		hp = 1;
		PickNewStartingPoint ();
		get_game_controller ();
//		while (hops < hop_limit) Shrink ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += speed*Time.deltaTime;
	}

	void PickNewStartingPoint(){
		transform.position = startingPoint + new Vector3 (Random.Range (xMin, xMax), Random.Range (yMin, yMax), Random.Range (zMin, zMax));
	}

	void OnTriggerEnter(Collider other) {
		if (in_game) {
			if (other.tag == "Bullet") {
				Destroy (other.gameObject);
				GC.ShotHit ();
				hp--;
			} else if (other.tag == "Player") {
				hp = 0;
				other.GetComponent<PlayerController> ().health -= hops;
				boom = explosion_chink;
			}
			if (hp <= 0) {
				loudDeath = true;
				Destroy (this.gameObject);
			}
		} else { //not in game space (yet?)
			loudDeath = false;
			if (other.tag == "Enemy") {
				if (hops < hop_limit) {
					Shrink ();
//					PickNewStartingPoint ();
				}
			}else if (other.tag == "GameSpace") {
				in_game = true;
			}
		}
	}

	void Shrink(){
		hops++;
		if(hp<hp_max) hp++;
		speed *= scale_factor;
		me.transform.localScale *= scale_factor;
		GetComponent<SphereCollider> ().transform.localScale *= scale_factor;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "GameSpace") {
			in_game = false;
			GC.MonsterEscaped ();
		}
	}

	void OnDestroy(){
		if (loudDeath) {
			GameObject boom_sound = (GameObject)Instantiate (boom, transform.position, transform.rotation);
			boom_sound.GetComponent<ExplosionController> ().SetHops(hops);

			GameObject ex_sprite = explosionSprite [Random.Range (0, explosionSprite.Length)];
			GameObject boom_sprite = (GameObject)Instantiate (ex_sprite, transform.position, ex_sprite.transform.rotation);
			boom_sprite.GetComponent<Rotator>().rotations.z = transform.GetComponent<Rigidbody>().angularVelocity.z;
			boom_sprite.transform.localScale *= hop_limit/hops;
		}
		if (in_game) {
			GC.MonsterDown ();
		}
	}

	public void MakeLoud(){
		loudDeath = true;
	}

	void get_game_controller(){
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameSpace");
		if (gameControllerObject != null)
		{
			GC = gameControllerObject.GetComponent <GameController>();
		}
		if (GC == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
}
