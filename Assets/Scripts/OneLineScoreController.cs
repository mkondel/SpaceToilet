using UnityEngine;
using System.Collections;
using UnityEngine.UI;

//OneLineScoreController class handles setting the values for one line in the top 10 scores board
public class OneLineScoreController : MonoBehaviour {

	public Text playerName;
	public Text time;
	public Text grade;
	public Text accuracy;
	public Text kills;

	public string NameStr {
		set {
			playerName.text = value;
		}
	}

	public string TimeStr {
		set {
			time.text = value;
		}
	}

	public string GradeStr {
		set {
			grade.text = value;
		}
	}

	public string AccuracyStr {
		set {
			accuracy.text = value;
		}
	}

	public string KillsStr {
		set {
			kills.text = value;
		}
	}

	//LoadFromOneString takes a string array and sets local private vars
	public void LoadFromOneStringArray(OneScoreFromTopTen oneScoreOnly){
		NameStr = oneScoreOnly.PlayerName;
		TimeStr = oneScoreOnly.TimeValueAsString();
		GradeStr = oneScoreOnly.GradeMe();
		AccuracyStr = oneScoreOnly.AccuracyValueAsString();
		KillsStr = oneScoreOnly.KillsValueAsString();
	}
}