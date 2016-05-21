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
	public float speed;
	public float Speed {
		get {
			return speed;
		}
		set {
			speed = value;
		}
	}

    public float tilt;
	public Boundary boundary;
	public GameObject shot;
	public Transform shotSpawn;
	public Text hzText;
	public Text healthText;
	public Animator muzzleFlash;
	public int health;
	public GameObject explosion;
	public GameObject explosionSound;

	private float fireRateHZ;
	private float nextFire;
	private int shots;
	public int Shots {get {return shots;}}

	void Start(){
		fireRateHZ = 1f;
		hzText.text = "1.00HZ";
		healthText.text = "Health: "+health;
	}

	void Update ()
	{
		healthText.text = "Health: "+health;

		if (Input.GetButton ("Fire1") && Time.time >= nextFire && Time.timeScale > 0) {
			if (fireRateHZ <= 0) {
				muzzleFlash.gameObject.SetActive (false);
				nextFire = Time.time + 1f;
			} else {
				nextFire = Time.time + (1f / fireRateHZ);
			}

			if (fireRateHZ > 1) {
				muzzleFlash.speed = fireRateHZ;
				muzzleFlash.gameObject.SetActive (true);
			} else {
				muzzleFlash.speed = 0;
			}

			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			shots++;
		} else if (Input.GetButtonUp ("Fire1")) {
			nextFire = Time.time;
			muzzleFlash.gameObject.SetActive(false);
		}

		if (health < 0) {
			//die
			healthText.text = "DEAD!";
			Instantiate (explosion, transform.position, transform.rotation);
			explosionSound.SetActive (true);
			explosionSound.transform.SetParent (null);
		}
	}

	void OnTriggerEnter(Collider other) {
		Destroy(other.gameObject);
	}

    void FixedUpdate ()
	{
//		float moveHorizontal = Input.GetAxis ("Horizontal");
//		float moveVertical = Input.GetAxis ("Vertical");
		float moveHorizontal = Input.GetAxis ("Mouse X");
//		float moveVertical = Input.GetAxis ("Mouse Y");

		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, 0.0f);//moveVertical);
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
		hzText.text = hz.ToString("n2")+"HZ";
	}
}