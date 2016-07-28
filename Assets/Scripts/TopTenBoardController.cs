using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TopTenBoardController class handles displaying of the top 10 scores at the end of a game
public class TopTenBoardController : MonoBehaviour {

	public OneLineScoreController[] topPlaces;

	private List<OneScoreFromTopTen> allTheTopScores;

	// Use this for initialization
	void Start () {
		ReloadFromCurrentSettings ();
	}

	//read from persister
	public void ReloadFromCurrentSettings(){
		allTheTopScores = GameObject.Find ("Persister").GetComponent<Persister>().settingsOfTheGame.topTenScores;
		for (int i = 0; i < allTheTopScores.Count; i++) {
			SetScoreForOneRow(i, allTheTopScores[i]);
		}
	}

	//set one of the top 10 scores
	//idx is the score index, scoreArray is a string[5] with [name,time,grade,accuracy,kills]
	void SetScoreForOneRow(int idx, OneScoreFromTopTen scoreArray){
		Debug.Log ("Top "+idx.ToString()+" score is "+scoreArray.ToString ());
		topPlaces[idx].LoadFromOneStringArray (scoreArray);
	}
}
