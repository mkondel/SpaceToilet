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
	public GameObject[] explosionPrefabs;

	protected GameController GC;
	protected bool in_game;
	protected int hops;
	protected int hp;
	public int Hp {get {return hp;} set {hp = value;}}
	protected Vector3 move_back_vector = new Vector3 (0,0,1);
	protected Vector3 original_speed;
	protected Vector3 original_scale;
	protected float original_radius;
	protected AudioSource oneEnemySoundSource;

	void Update () {
		transform.position += speed*Time.deltaTime;
		if (hp <= 0) {
			Destroy (this.gameObject);
		}
	}

	protected void Shrink(){
		hops++;
		if(hp<hp_max) hp++;
		speed *= scale_factor;
		myBodyObject.transform.localScale *= scale_factor;
		GetComponent<SphereCollider> ().radius *= scale_factor;
	}

	protected void Explode(){ 
		// must instantiate with GameObjet in explosionSprite.  need to inst new obj, add sprite prop, set sprite to my sprite from array, bingo!
		GameObject explosionPrefab = explosionPrefabs [UnityEngine.Random.Range (0, explosionPrefabs.Length)];

		GameObject myExplosion = (GameObject)Instantiate(explosionPrefab, transform.position, explosionPrefab.transform.rotation);

		myExplosion.GetComponent<Rotator> ().rotations.z = transform.GetComponent<Rigidbody> ().angularVelocity.z;

		myExplosion.transform.localScale *= hop_limit / hops;
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
