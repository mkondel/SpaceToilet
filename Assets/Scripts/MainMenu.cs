using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void BackToMenu(){
		SceneManager.LoadScene(0);
	}

	public void RestartShooter(){
		if (SceneManager.sceneCount == 1) {
			Debug.Log ("reloading the only scene, which should be the shooter.  for testing...");
			SceneManager.LoadScene (SceneManager.GetActiveScene().name);
		} else if (SceneManager.sceneCount > 1) {
			Debug.Log ("reloading shooter scene normally.");
			SceneManager.LoadScene (1);
		} else {
			Debug.Log ("Cant load the scene needed to restart shooter...");
		}
	}
		
	public void StartShooter(){
		SceneManager.LoadScene (1);
	}
}
