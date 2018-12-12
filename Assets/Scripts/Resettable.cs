using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resettable : MonoBehaviour 
{
	Vector3 start_pos;
	Quaternion start_rot;
	Vector3 start_scale;

	void Awake()
	{
		start_pos = transform.position;
		start_rot = transform.localRotation;
		start_scale = transform.localScale;
	}	

	public void ResetTransform()
	{
		transform.position = start_pos;
		transform.localRotation = start_rot;
		transform.localScale = start_scale;
	}
}
