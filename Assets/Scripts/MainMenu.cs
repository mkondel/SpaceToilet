using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void BackToMenu(){
		SceneManager.LoadScene(0);
	}

	public void RestartShooter(){
		SceneManager.LoadScene (1);
	}
		
	public void StartShooter(){
		SceneManager.LoadScene (1);
	}
}
