using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreBoardController : MonoBehaviour {

	public Text shots_text;
	public Text grade_text;
	public Text kills_text;

	public void ShowScores(string grade="XX", int kills=0, int total=0, int shots=0, int hits=0){

		float accuracy;
		if (hits > 0) {
			accuracy = (float)hits/shots;	
		} else {
			accuracy = 0;
		}
		kills_text.text = "KILLED\n"+ kills.ToString () +"/"+total.ToString();
		grade_text.text = grade;
		shots_text.text = "ACCURACY\n"+ accuracy.ToString ("n2");
	}
}
