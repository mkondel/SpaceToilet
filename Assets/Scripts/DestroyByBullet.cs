using UnityEngine;
using System.Collections;

public class DestroyByBullet : MonoBehaviour {

	public string enemyTag;
	public string gameSpaceTag;

	void OnTriggerEnter(Collider other)
	{
		if (other.tag == enemyTag) {
//			other.GetComponent<EnemyAController>().Hp--;
			other.GetComponent<EnemyController>().Hp--;
			Destroy (this.gameObject);
		}
	}

	void OnTriggerExit(Collider other){
		if (other.tag == gameSpaceTag) {
			Destroy (this.gameObject);
		}
	}
}
