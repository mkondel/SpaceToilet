using UnityEngine;
using System.Collections;

public class EnemyAController : MonoBehaviour {

	public Vector3 speed;
	public float xMin, xMax, yMin, yMax, zMin, zMax;
	public Vector3 startingPoint;
	public GameObject explosion;
	public GameObject[] explosionSprite;
	public int hop_limit;
	public GameObject me;
	public float scale_factor;
	public int hp_max;

	private GameController GC;
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
		boom = explosion;
		PickNewStartingPoint ();
		//		while (hops < hop_limit) Shrink ();
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += speed*Time.deltaTime;
		if (hp <= 0) {
			Destroy (this.gameObject);
		}
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
		}
	}

	void OnDestroy(){
		if (in_game) {
			GotMyselfKilled ();

			if (boom) {
				Instantiate (boom, transform.position, transform.rotation);
			} else {
				Debug.Log ("EnemyA explosion sound is absent");
			}
//			ExplosionController boom_sound = Instantiate (boom, transform.position, transform.rotation) as ExplosionController;
//			boom_sound.SetHops (hops);

			GameObject ex_sprite = explosionSprite [Random.Range (0, explosionSprite.Length)];
			GameObject boom_sprite = (GameObject)Instantiate (ex_sprite, transform.position, ex_sprite.transform.rotation);
			boom_sprite.GetComponent<Rotator> ().rotations.z = transform.GetComponent<Rigidbody> ().angularVelocity.z;
			boom_sprite.transform.localScale *= hop_limit / hops;
		}
		else
			Escaped ();
	}

	void Escaped (){
		// ???
		GC.MonsterEscaped ();
//		Debug.Log("Monster escaped ->"+transform.position);
	}

	void GotMyselfKilled(){
		GC.MonsterKilled ();
//		Debug.Log("Monster got killed ->"+transform.position);
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
