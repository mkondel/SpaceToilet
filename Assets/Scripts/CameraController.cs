using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
	
	public Color target;

	[Range (0.01f, 10f)]
	[SerializeField]
	public float pulse;

	[Range (0f, 1f)]
	[SerializeField]
	public float lerp_slerp;


	private Camera cam;
	private Color start;

	// Use this for initialization
	void Start () {
		cam = this.GetComponent<Camera> ();
		start = cam.backgroundColor;
	}
	
	// Update is called once per frame
	void Update () {
		float t = Mathf.PingPong(Time.time, pulse) / pulse;
		Color slerp = (Vector4)Vector3.Slerp((Vector4)start, (Vector4)target, t);
		Color lerp = Color.Lerp(start, target, t);
		cam.backgroundColor = slerp * lerp_slerp + lerp * (1 - lerp_slerp);
	}
}
