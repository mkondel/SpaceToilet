using System;
using UnityEngine;

public abstract class EnemyController: MonoBehaviour{
	public Vector3 speed;
	public float xMin, xMax, yMin, yMax, zMin, zMax;
	public Vector3 startingPoint;
	public int hop_limit;
	public GameObject myBodyObject;
	public float scale_factor;
	public int hp_max;
	public AudioClip[] deathSounds;
	public float notes = 1.059463f;
	public Sprite[] explosionSprites;
	public GameObject explosionPrefab;

	protected GameController GC;
	protected bool in_game;
	protected int hops;
	protected int hp;
	public int Hp {get {return hp;} set {hp = value;}}
	protected Vector3 move_back_vector = new Vector3 (0,0,1);
	protected Vector3 original_speed;
	protected Vector3 original_scale;
	protected float original_radius;
	protected Sprite exlosionSprite;
	protected GameObject explosionObject;
	protected AudioSource oneEnemySoundSource;


	void GetGameController(){}
	void GetOneAudioSource(){}
	void PickNewStartingPoint(){}
	void Reset(){}
	void OnTriggerEnter(Collider other){}
	void OnTriggerStay(Collider other){}
	void OnTriggerExit(Collider other){}
	void OnDestroy(){}

	protected void Shrink(){
		hops++;
		if(hp<hp_max) hp++;
		speed *= scale_factor;
		myBodyObject.transform.localScale *= scale_factor;
		GetComponent<SphereCollider> ().radius *= scale_factor;
	}

	protected void Explode(){
		exlosionSprite = explosionSprites[UnityEngine.Random.Range (0, explosionSprites.Length)];
		explosionObject = (GameObject)Instantiate (exlosionSprite, transform.position, transform.rotation);
		explosionObject.GetComponent<Rotator> ().rotations.z = transform.GetComponent<Rigidbody> ().angularVelocity.z;
		explosionObject.transform.localScale *= hop_limit / hops;
	}

	protected void MakeDeathSound(){
		if (oneEnemySoundSource && oneEnemySoundSource.isActiveAndEnabled) {
			oneEnemySoundSource.Stop ();
			oneEnemySoundSource.transform.position = transform.position;
			oneEnemySoundSource.pitch = Mathf.Pow (notes, hops);
			oneEnemySoundSource.PlayOneShot (deathSounds [UnityEngine.Random.Range (0, deathSounds.Length)]);
		}
	}
}
