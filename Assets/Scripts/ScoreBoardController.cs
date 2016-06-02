using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoardController : MonoBehaviour {

	public Text accText;
	public Text gradeText;
	public Text killsText;

	public void ShowScores(string grade="XX", int kills=0, int total=0, int shots=0, int hits=0){

		float accuracy;
		if (hits > 0) {
			accuracy = (float)hits/shots;	
		} else {
			accuracy = 0;
		}
		killsText.text = "KILLED\n"+ kills.ToString () +"/"+total.ToString();
		gradeText.text = grade;
		accText.text = "ACCURACY\n"+ accuracy.ToString ("n2");
	}
}
