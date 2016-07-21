using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GyroToText : MonoBehaviour {

	public Slider debugSlider;
	public Text debugText;
	public Vector3 R {
		get {
			return r;
		}
		set {
			r = value;
			SetOtherValues ();
		}
	}

	public Vector3 L {
		get {
			return l;
		}
		set {
			l = value;
			SetOtherValues ();
		}
	}

	private Vector3 l;	//left most limit vector for tilt
	private Vector3 r;  //right most limit vector for tilt
	private Vector3 C;	//middle vector between L and R
	private float   m;	//half of the angle between L and R
	private Vector3 n;	//normal to the plane defined by L and R
	private Vector3 bakL, bakR;

	public void BackupLR(){
		bakL = l;
		bakR = r;
	}
	public void RestoreLR(){
		L = bakL;
		R = bakR;
	}

	void Start(){
		#if UNITY_ANDROID || UNITY_IOS
			Input.gyro.enabled = true;
		#endif
	}

	void Update(){
		#if UNITY_ANDROID || UNITY_IOS
			if (debugSlider) {
				UserDefinedTilt ();
			}
		#endif
	}

	//UserDefinedTilt returns [-1,1] float range
	//maps input value from gyroscope output
	//range of tilt can be user defined in options
	public float UserDefinedTilt(){
		Vector3 inputVector = Input.gyro.gravity;
		Vector3 planeVector = Vector3.ProjectOnPlane (inputVector, n);
		float c = DetermineSignedAngle(planeVector);
		float finalAnswer = -Mathf.Clamp (c / m, -1f, 1f);
		if (debugSlider && debugText) {
			debugSlider.value = finalAnswer;
			debugText.text = l.ToString () + "\n" + r.ToString ();
		}
		return finalAnswer;
	}

	//SetOtherValues updates tilt vectors if user does the calibration in the options menu
	private void SetOtherValues(){
		C = Vector3.Slerp (l, r, 0.5f);
		m = Vector3.Angle (l, r) / 2f;
		n = Vector3.Cross (l, r);
	}

	//SetL assigns a new value to the left most tilt vector
	public void SetL(){
		L = Input.gyro.gravity;
	}

	//SetR assigns a new value to the right most tilt vector
	public void SetR(){
		R = Input.gyro.gravity;
	}

	//FullTiltReset resets all the vectors for tilt back to starting values
	public void FullTiltReset(){
//		Vector3 vecD = new Vector3 (0.01f, 0.01f, 0.01f);
		l = Vector3.left;// + vecD;
		r = Vector3.right;// - vecD;
		SetOtherValues ();
	}

	//LoadLR sets the values for L and R given from the persister
	public void LoadRL(Vector3 loadL, Vector3 loadR){
		l = loadL;
		r = loadR;
		SetOtherValues ();
	}

	//DetermineSignedAngle returns the value of the angle in a plane, used for tilt controls
	float DetermineSignedAngle(Vector3 v){
		Vector3 crossProd = Vector3.Cross (v, C);
		float angle = Vector3.Angle (v, C);
		float dotProd = Vector3.Dot (crossProd, n);
		return angle * Mathf.Sign (dotProd);
	}
}
