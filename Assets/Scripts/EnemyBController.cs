using UnityEngine;
using System.Collections;

public class EnemyBController : MonoBehaviour {

	public Vector3 speed;
	public float xMin, xMax, yMin, yMax, zMin, zMax;
	public Vector3 startingPoint;
	public int hop_limit;
	public GameObject myBodyObject;
	public float scale_factor;
	public int hp_max;
	public AudioSource oneEnemySoundSource;
	public AudioClip[] deathSounds;
	public float notes = 1.059463f;
	public Sprite[] explosionSprites;
	public GameObject explosionPrefab;

	private GameController GC;
	private bool in_game;
	private int hops;
	private int hp;
	public int Hp {get {return hp;} set {hp = value;}}
	private Vector3 move_back_vector = new Vector3 (0,0,1);
	private Vector3 original_speed;
	private Vector3 original_scale;
	private float original_radius;
	private Sprite exlosionSprite;
	private GameObject explosionObject;

	// Use this for initialization
	void Start () {
		get_game_controller ();
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
		myBodyObject.transform.localScale *= scale_factor;
		GetComponent<SphereCollider> ().radius *= scale_factor;
	}

	void Explode(){
		exlosionSprite = explosionSprites[Random.Range (0, explosionSprites.Length)];
		explosionObject = (GameObject)Instantiate (exlosionSprite, transform.position, transform.rotation);
		explosionObject.GetComponent<Rotator> ().rotations.z = transform.GetComponent<Rigidbody> ().angularVelocity.z;
		explosionObject.transform.localScale *= hop_limit / hops;
	}

	void MakeDeathSound(){
		if (oneEnemySoundSource && oneEnemySoundSource.isActiveAndEnabled) {
			oneEnemySoundSource.Stop ();
			oneEnemySoundSource.transform.position = transform.position;
			oneEnemySoundSource.pitch *= Mathf.Pow (notes, hops);
			oneEnemySoundSource.PlayOneShot (deathSounds [Random.Range (0, deathSounds.Length)]);
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

	void get_game_controller(){
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameSpace");
		if (gameControllerObject != null){
			GC = gameControllerObject.GetComponent <GameController>();
		}
		if (GC == null){
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
}
