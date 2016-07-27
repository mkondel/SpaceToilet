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

	private string nameStr;

	public string NameStr {
		get {
			return nameStr;
		}
		set {
			nameStr = value;
			playerName.text = value;
		}
	}

	private string timeStr;

	public string TimeStr {
		get {
			return timeStr;
		}
		set {
			timeStr = value;
			time.text = value;
		}
	}

	private string gradeStr;

	public string GradeStr {
		get {
			return gradeStr;
		}
		set {
			gradeStr = value;
			grade.text = value;
		}
	}

	private string accuracyStr;

	public string AccuracyStr {
		get {
			return accuracyStr;
		}
		set {
			accuracyStr = value;
			accuracy.text = value;
		}
	}

	private string killsStr;

	public string KillsStr {
		get {
			return killsStr;
		}
		set {
			killsStr = value;
			kills.text = value;
		}
	}

	// Use this for initialization
	void Start () {
	
	}

//	LoadFromOneString takes a string array and sets local private vars
	public void LoadFromOneStringArray(string[] allInOne){
		NameStr = allInOne[0];
		TimeStr = allInOne[1];
		GradeStr = allInOne[2];
		AccuracyStr = allInOne[3];
		KillsStr = allInOne[4];
	}
}
