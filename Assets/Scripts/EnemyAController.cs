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

	public int Hp {get {return hp;} set {hp = value;}}

	private GameObject boom;
	private Vector3 move_back_vector = new Vector3 (0,0,1);
	private Vector3 original_speed;
	private Vector3 original_scale;
	private float original_radius;

	// Use this for initialization
	void Start () {
		get_game_controller ();
		original_speed = speed;
		original_scale = me.transform.localScale;
		original_radius = GetComponent<SphereCollider> ().radius;
		Reset ();
	}

	void Reset(){
		speed = original_speed;
		me.transform.localScale = original_scale;
		GetComponent<SphereCollider> ().radius = original_radius;
		hops = 1;
		hp = 1;
		in_game = false;
		loudDeath = true;
		boom = explosion;
		PickNewStartingPoint ();
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
//				Destroy (other.gameObject);
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
					//					PickNewStartingPoint ();
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

	void Shrink(){
		hops++;
		if(hp<hp_max) hp++;
		speed *= scale_factor;
		me.transform.localScale *= scale_factor;
		GetComponent<SphereCollider> ().radius *= scale_factor;
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
