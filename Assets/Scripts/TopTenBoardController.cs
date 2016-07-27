using UnityEngine;
using System.Collections;

//TopTenBoardController class handles displaying of the top 10 scores at the end of a game
public class TopTenBoardController : MonoBehaviour {

	public OneLineScoreController[] topPlaces;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < topPlaces.Length; i++) {
			SetRandomScore (i);
		}
	}


	void SetRandomScore(int idx){
		string[] fakeStringArray = new string[5];
		fakeStringArray [0] = "Fake Name "+Random.Range(1,99999).ToString();
		fakeStringArray [1] = Random.Range(1,99).ToString()+":"+Random.Range(1,99).ToString()+":"+Random.Range(1,999).ToString()+"ms";
		fakeStringArray [2] = "A";
		fakeStringArray [3] = Random.Range(1,99).ToString()+"%";
		fakeStringArray [4] = Random.Range(1,99).ToString()+"%";
		Debug.Log (fakeStringArray.ToString ());
		topPlaces[idx].LoadFromOneStringArray (fakeStringArray);
	}
}
