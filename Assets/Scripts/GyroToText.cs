using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyroToText : MonoBehaviour {

//	public Text halfMaxAngleText;
//	public Text angleText;
//	public Text dotText;
//	public Text projText;
//	public Text Ltext;
//	public Text Ctext;
//	public Text Rtext;

	private Vector3 L = Vector3.zero;	//left most limit vector for tilt
	private Vector3 R = Vector3.zero;	//right most limit vector for tilt
	private Vector3 C = Vector3.zero;	//middle vector between L and R
	private float   m = 0f;				//half of the angle between L and R
	private Vector3 n = Vector3.zero;	//normal to the plane defined by L and R

	void Start(){
		#if UNITY_ANDROID || UNITY_IOS
			Input.gyro.enabled = true;
		#endif
	}

	void Update(){
		#if UNITY_ANDROID || UNITY_IOS
			UserDefinedTilt ();
		#endif
	}

	//UserDefinedTilt returns [-1,1] float range
	//maps input value from gyroscope output
	//range of tilt can be user defined in options
	float UserDefinedTilt(){
		Vector3 inputVector = Input.gyro.gravity;
		Vector3 planeVector = Vector3.ProjectOnPlane (inputVector, n);
//		float inputDotCenter = Vector3.Dot (inputVector, C);
		float c = DetermineSignedAngle(planeVector);
		float finalAnswer = Mathf.Clamp (c / m, -1f, 1f);

//		angleText.text = c.ToString();
//		halfMaxAngleText.text = m.ToString ();
//		dotText.text = inputDotCenter.ToString();
//		projText.text = finalAnswer.ToString();

		return finalAnswer;
	}

	//SetOtherValues updates tilt vectors if user does the calibration in the options menu
	private void SetOtherValues(){
		C = Vector3.Slerp (L, R, 0.5f);
		m = Vector3.Angle (L, R) / 2f;
		n = Vector3.Cross (L, R);
//		Ctext.text = C.ToString ();
	}

	//SetL assigns a new value to the left most tilt vector
	public void SetL(){
		L = Input.gyro.gravity;
		SetOtherValues ();
//		Ltext.text = L.ToString ();
	}

	//SetR assigns a new value to the right most tilt vector
	public void SetR(){
		R = Input.gyro.gravity;
		SetOtherValues ();
//		Rtext.text = R.ToString ();
	}

	//FullTiltReset resets all the vectors for tilt back to starting values
	public void FullTiltReset(){
		L = Vector3.zero;
		C = Vector3.zero;
		R = Vector3.zero;
		m = 0f;
		n = Vector3.zero;
	}

	//DetermineSignedAngle returns the value of the angle in a plane, used for tilt controls
	float DetermineSignedAngle(Vector3 v){
		Vector3 crossProd = Vector3.Cross (v, C);
		float angle = Vector3.Angle (v, C);
		float dotProd = Vector3.Dot (crossProd, n);
		return angle * Mathf.Sign (dotProd);
	}
}
