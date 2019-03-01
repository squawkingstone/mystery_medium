using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugCamera : MonoBehaviour 
{
	[SerializeField] float move_speed;
	[SerializeField] float sprint_speed;
	[SerializeField] float mouse_sensitivity;

	float xrot, yrot;

	float h, v, a, mx, my;

	void Start()
	{
		xrot = yrot = 0;
	}

	void Update()
	{
		h = Input.GetAxis("Horizontal");
		v = Input.GetAxis("Vertical");
		a = Input.GetAxis("Ascend");
		mx = Input.GetAxis("Mouse X");
		my = Input.GetAxis("Mouse Y");

		xrot -= my * mouse_sensitivity;
		xrot = Mathf.Clamp(xrot, -85f, 85f);

		yrot += mx * mouse_sensitivity;
		if (yrot > 360f) yrot -= 360f;
		if (yrot < 0f)   yrot += 360f;

		transform.localRotation = Quaternion.Euler(xrot, yrot, 0f);

		bool shift = Input.GetKey(KeyCode.LeftShift);

		Vector3 move = (transform.forward * v) + (transform.right * h) + (Vector3.up * a);
		
		transform.position += move * ((shift) ? sprint_speed : move_speed) * Time.deltaTime;
	}
}
