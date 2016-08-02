using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextByOS : MonoBehaviour {

	public static string[] osList = new string[5]{"Linux","OSX","Android","iOS","Windows"};
	[Multiline]
	public string[] textList = new string[5]{"Linux text","OSX text","Android text","iOS text","Windows text"};

	// Use this for initialization
	void Start () {
		Text myText = GetComponent<Text> ();
		int idx = -1;

		#if UNITY_STANDALONE_LINUX|| UNITY_EDITOR_LINUX
			idx = 0;
		#elif UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX
			idx = 1;
		#elif UNITY_ANDROID
			idx = 2;
		#elif UNITY_IOS || UNITY_IPHONE
			idx = 3;
		#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
			idx = 4;
		#endif

		myText.text = textList[idx];
	}
}
