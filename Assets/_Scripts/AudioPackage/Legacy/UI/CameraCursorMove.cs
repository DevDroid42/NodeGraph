using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCursorMove : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Cursor.lockState = CursorLockMode.Confined;
		OriginalPos = gameObject.transform.position;
	}
	public bool PosDebug;
	public Vector3 OriginalPos;
	public Vector3 offset;
	private void Awake()
	{
		OriginalPos = gameObject.transform.position;
	}

	public float sensitivity = 0.05f;
	public Transform Target;
	void LateUpdate () {
		Camera mycam = GetComponent<Camera>();
		if (!PosDebug)
		{
			offset = mycam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane));
			offset.x -= 0.5f;
			offset.y -= 0.5f;
			offset.x *= sensitivity;
			offset.y *= sensitivity;
			offset.x += 0.5f;
			offset.y += 0.5f;
		}
		Vector3 FinalPos = OriginalPos + offset;
		Vector3 Difference = new Vector3((FinalPos.x - transform.position.x) - 0.5f, (FinalPos.y - transform.position.y) - 0.5f);
		gameObject.transform.Translate((Difference)*Time.deltaTime);
		transform.LookAt(Target, Vector3.up);        
	}
}
