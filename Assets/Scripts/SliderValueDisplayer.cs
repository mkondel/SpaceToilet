using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SliderValueDisplayer : MonoBehaviour {

	public Text my_value_text;
	public string label_text = "Mouse Sensitivity ";

	public void Value(float newval){
		my_value_text.text = label_text + newval.ToString("n2");
	}

	void Awake(){
		my_value_text.text = label_text + GetComponent<Slider> ().value.ToString("n1");
	}
}
