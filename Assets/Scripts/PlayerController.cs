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
	public GameObject hz_text;
	public GameObject health_text;
	public GameObject muzzle_flash;
	public int health;
	public GameObject explosion;
	public GameObject explosion_sound;

	private float fireRateHZ;
	private float nextFire;
	private GameController GC;

	void Start(){
		get_game_controller ();
		fireRateHZ = 1.0f;
		hz_text.GetComponent<Text>().text = "1.00HZ";
		health_text.GetComponent<Text>().text = "Health: "+health;
	}

	void Update ()
	{
		health_text.GetComponent<Text>().text = "Health: "+health;

		if (Input.GetButton ("Fire1") && Time.time >= nextFire && Time.timeScale > 0) {
			nextFire = Time.time + (1.0f/fireRateHZ);
			Instantiate (shot, shotSpawn.position, shotSpawn.rotation);
			GameObject foo = (GameObject)Instantiate (muzzle_flash, shotSpawn.position, muzzle_flash.transform.rotation);
			foo.transform.parent = transform;
			GC.ShotFired ();
		} else if (Input.GetButtonUp ("Fire1")) {
			nextFire = Time.time;
		}

		if (health < 0) {
			//die
			health_text.GetComponent<Text>().text = "DEAD!";
			Instantiate (explosion, transform.position, transform.rotation);
			explosion_sound.SetActive (true);
			explosion_sound.transform.SetParent (null);
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

	public void SetFireRateHZ(float hz){
		fireRateHZ = hz;
		hz_text.GetComponent<Text>().text = hz.ToString("n2")+"HZ";
	}
}