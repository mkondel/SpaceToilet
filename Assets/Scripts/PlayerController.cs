using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax;
}

public class PlayerController : MonoBehaviour
{
	public int health;
	public float speed;
    public float tilt;
	public int KHz;
	public Text hzText;
	public Text healthText;
	public Boundary boundary;
	public Transform shotSpawn;
	public Animator muzzleFlashBullet;
	public Animator muzzleFlashPlasma;
	public GameObject[] shot;
	public GameObject deathAnimation;
	public AudioSource deathSound;
	public AudioSource damageSound;
	public int Shots {get {return shots;}}

	private float fireRateHZ;
	private float nextFire;
	private int shots;
	private Animator muzzleAnimation;

	void Start(){
		fireRateHZ = 1f;
		hzText.text = "1.00HZ";
		healthText.text = "Health: "+health;
		muzzleFlashBullet.StartPlayback ();
		muzzleFlashPlasma.StartPlayback ();
		muzzleAnimation = muzzleFlashBullet;
	}

	void Update (){
		//Keep health display current
		healthText.text = "Health: "+health;

		if (Input.GetButton ("Fire1") && Time.time >= nextFire && Time.timeScale > 0) {
			//Fire button is pressed down AND (fire timeout has passed) AND (the game is not paused)
			FireWeapon();	//FIRE THE WEAPON!!!
		} else if (Input.GetButtonUp ("Fire1")) {
//			nextFire = Time.time;
			MuzzleOff();
		}

		if (health <= 0) {
		//die
			healthText.text = "DEAD!";
			Instantiate (deathAnimation, transform.position, transform.rotation);
			if (deathSound && deathSound.enabled) {
				deathSound.Play();
				deathSound.transform.SetParent (null);
//				Destroy (this.gameObject);
			}
		}
	}
		
	void FireWeapon(){
		GameObject s;
		if (fireRateHZ < KHz && fireRateHZ > 0) {
			s = shot [0];
			nextFire = Time.time + (1f / fireRateHZ);	//fire timeout is fraction of a second
			if (fireRateHZ > 1) {
				muzzleFlashBullet.speed = fireRateHZ;		//muzzle flash animation loop speed == fire rate
			}else{
				muzzleFlashBullet.speed = 1;
			}
			muzzleAnimation = muzzleFlashBullet;
		} else {
			s = shot [1];
			nextFire = Time.time + 1f;					//fire timeout is forced to be 1 second
			muzzleAnimation = muzzleFlashPlasma;
		}

		if (s != null) {
			MuzzleOff ();
			muzzleAnimation.speed = fireRateHZ;		//muzzle flash animation loop speed == fire rate
			muzzleAnimation.gameObject.SetActive(true);
			Instantiate (s, shotSpawn.position, shotSpawn.rotation);
			shots++;
		} else {
			Debug.LogError ("Weapons not setup in player?");
		}
	}

	void MuzzleOff(){
		muzzleFlashBullet.gameObject.SetActive (false);
		muzzleFlashPlasma.gameObject.SetActive (false);
	}

	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
	}

    void FixedUpdate ()
	{
		float moveHorizontal = Input.GetAxis ("Mouse X");
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);
        GetComponent<Rigidbody>().velocity = movement * speed;

        GetComponent<Rigidbody>().position = new Vector3 
        (
            Mathf.Clamp (GetComponent<Rigidbody>().position.x, boundary.xMin, boundary.xMax), 
            0.0f, 
			GetComponent<Rigidbody>().position.z
        );

        GetComponent<Rigidbody>().rotation = Quaternion.Euler (0.0f, 0.0f, GetComponent<Rigidbody>().velocity.x * -tilt);
    }

	public void SetFireRateHZ(float hz){
		fireRateHZ = hz;
		if (fireRateHZ < KHz) {
			hzText.text = hz.ToString ("n2") + "HZ";
		} else {
			hzText.text = "Killa' HZ";
		}
	}

	public int TakeDamage(int dmg){
		if (dmg != 0) {
			if (damageSound && damageSound.isActiveAndEnabled) {
				damageSound.Play ();
			}
			health -= dmg;
			if (health < 0) {
				health = 0;
			}
		}
		return health;
	}
}