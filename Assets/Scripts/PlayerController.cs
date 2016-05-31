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
	public int Shots {get {return shots;}}
	public AudioClip bulletShotSoundClip;
	public AudioClip plasmaShotSoundClip;
	public AudioSource damageAudioSource;
	public AudioClip damageSoundClip;
	public AudioClip deathSoundClip;

	public GameObject myBody, myLeftEng, myRightEng, myGlass;

	private float fireRateHZ;
	private float nextFire;
	private int shots;
	private Animator muzzleAnimation;
	private static WaitForSeconds plasmaTimeout = new WaitForSeconds (0.5f);
	private AudioSource shipAudioSource;

	void Start(){
		fireRateHZ = 1f;
		hzText.text = "1.00HZ";
		healthText.text = "Health: "+health;
		muzzleFlashBullet.StartPlayback ();
		muzzleFlashPlasma.StartPlayback ();
		muzzleAnimation = muzzleFlashBullet;
		shipAudioSource = GetComponent<AudioSource> ();

		#if UNITY_ANDROID
			myBody.GetComponent<MeshRenderer>().material = Resources.Load("SilverMaterialAndroid", typeof(Material)) as Material;
			myGlass.GetComponent<MeshRenderer>().material = Resources.Load("GlassMaterialAndroid", typeof(Material)) as Material;
			myLeftEng.GetComponent<MeshRenderer>().material = Resources.Load("RedMaterialAndroid", typeof(Material)) as Material;
			myRightEng.GetComponent<MeshRenderer>().material = Resources.Load("RedMaterialAndroid", typeof(Material)) as Material;
		#endif
	}

	void Update (){
		//Keep health display current
		healthText.text = "Health: "+health;

		//Fire button is pressed down AND (fire timeout has passed) AND (the game is not paused)
		if (Input.GetButton ("Fire1") && Time.time >= nextFire && Time.timeScale > 0) {
			FireWeapon();	//FIRE THE WEAPON!!!
		//Player lets go of fire button
		} else if (Input.GetButtonUp ("Fire1")) {
			MuzzleOff();
		}

		if (health <= 0) {
		//die
			healthText.text = "DEAD!";
			Instantiate (deathAnimation, transform.position, transform.rotation);
			if (damageAudioSource) {
				damageAudioSource.clip = deathSoundClip;
				damageAudioSource.Play();
				damageAudioSource.transform.SetParent (null);
			}
		}
	}
		
	void FireWeapon(){
		//local object used to hold the currently fired shot
		GameObject s;

		//fire the bullet shot
		if (fireRateHZ < KHz && fireRateHZ > 0) {
			s = shot [0];									//choose the bullet prefab
			nextFire = Time.time + (1f / fireRateHZ);		//fire timeout is fraction of a second
			if (fireRateHZ > 1) {
				muzzleFlashBullet.speed = fireRateHZ;		//muzzle flash animation loop speed == fire rate
			}else{
				muzzleFlashBullet.speed = 1;				//muzzle flash anim speed is forced to be 1 if actual Hz falls below
			}
			muzzleAnimation = muzzleFlashBullet;				//muzzle animation is the bullet one (8bit-style)
			muzzleFlashPlasma.gameObject.SetActive (false);		//disable the muzzle animation for plasma, in case it was playing before
			if (shipAudioSource && shipAudioSource.isActiveAndEnabled) {
				shipAudioSource.PlayOneShot(bulletShotSoundClip);	//play the audio clip for the bullet shot
			}

		//fire the plasma shot
		} else {
			s = shot [1];										//choose the plasma prefab
			nextFire = Time.time + 1f;							//fire timeout is forced to be 1 second
			muzzleAnimation = muzzleFlashPlasma;				//use the muzzle anim for plasma
			muzzleFlashBullet.gameObject.SetActive (false);		//disable the bullet flash anim
			if (shipAudioSource && shipAudioSource.isActiveAndEnabled) {
				shipAudioSource.PlayOneShot(plasmaShotSoundClip);	//play the plasma shot sound
			}
			StartCoroutine (DelayMuzzleOff());					//turn off the plasmaball object after some delay.  makes better shot effect...
		}

		//common to both types of shots
		if (s != null) {
			muzzleAnimation.gameObject.SetActive(true);					//play muzzle animation
			Instantiate (s, shotSpawn.position, shotSpawn.rotation);	//instantiate whatever shot prefab is needed
			shots++;													//increment number of shots fired
		} else {
			Debug.LogError ("Weapons not setup in player?");
		}
	}


	//turns off muzzle flash after the predefined timeout
	IEnumerator DelayMuzzleOff(){
		yield return plasmaTimeout;
		MuzzleOff ();
	}


	//Both Sound and Animation for shooting weapons stop
	void MuzzleOff(){
		if (shipAudioSource && !shipAudioSource.isPlaying) {
			shipAudioSource.Stop ();
		}
		muzzleAnimation.gameObject.SetActive (false);
	}

	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
	}

    void FixedUpdate (){
		float moveHorizontal = 0;

		#if UNITY_STANDALONE || UNITY_EDITOR
			moveHorizontal = Input.GetAxis ("Mouse X");
		#elif UNITY_ANDROID
			moveHorizontal = Input.acceleration.x * 75.0f;
		#endif

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
			if (damageAudioSource && damageAudioSource.isActiveAndEnabled) {
				damageAudioSource.clip = damageSoundClip;
				damageAudioSource.Play ();
			}
			health -= dmg;
			if (health < 0) {
				health = 0;
			}
		}
		return health;
	}
}