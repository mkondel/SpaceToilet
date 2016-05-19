using UnityEngine;
using System.Collections;

public class DestroyByBullet : MonoBehaviour {

	public string enemyTag;
	public string gameSpaceTag;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == enemyTag) {
			Destroy (other.gameObject);
		}

		if (other.tag != gameSpaceTag) {
			Destroy (this.gameObject);
		}
	}
}
