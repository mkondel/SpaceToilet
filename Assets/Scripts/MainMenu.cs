using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	
	private static WaitForSeconds waitingTime;
	private float waitTime = 0.5f;

	public void BackToMenu(){
		StartCoroutine( LoadShooterSceneDelayed(0) ) ;
	}

	public void StartShooter(){
		StartCoroutine( LoadShooterSceneDelayed(1) ) ;
	}

	public void RestartShooter(){
		if (SceneManager.sceneCount < 1) {
			Debug.Log ("Cant load the scene needed to restart shooter...");
		} else {
			Debug.Log ("reloading shooter scene normally.");
			StartShooter();
		}
	}

	IEnumerator LoadShooterSceneDelayed(int sceneIndex) {
		waitingTime = new WaitForSeconds (waitTime);
		yield return waitingTime;
		Debug.Log ("Changing to Shooter Scene");
		SceneManager.LoadScene (sceneIndex);
	}
}
