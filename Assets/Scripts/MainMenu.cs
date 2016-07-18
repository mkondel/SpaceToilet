using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {
	
	private static WaitForSeconds waitingTime;
	private float waitTime = 1f;

	public void BackToMenu(){
		StartCoroutine( LoadSceneDelayed(0) ) ;
		Time.timeScale = 1;		//unpause
	}

	public void StartShooter(){
		StartCoroutine( LoadSceneDelayed(1) ) ;
	}

	public void RestartShooter(){
		if (SceneManager.sceneCount < 1) {
			Debug.Log ("Cant load the scene needed to restart shooter...");
		} else {
			Debug.Log ("reloading shooter scene normally.");
			StartShooter();
		}
	}

	IEnumerator LoadSceneDelayed(int sceneIndex) {
		waitingTime = new WaitForSeconds (waitTime);
		yield return waitingTime;
		Debug.Log ("Changing to Shooter Scene");
		SceneManager.LoadScene (sceneIndex);
	}
}
