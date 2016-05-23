using UnityEngine;
using System.Collections;

public class DestroyByLightning : MonoBehaviour {
	void OnTriggerEnter(Collider other){
		other.GetComponent<EnemyAController>().Hp = 0;
	}
}
