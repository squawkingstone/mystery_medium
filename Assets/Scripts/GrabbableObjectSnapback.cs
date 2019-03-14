using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectSnapback : MonoBehaviour 
{
	Vector3 position;
	Quaternion rotation;
	OVRGrabbable grabbable;
	Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		grabbable = GetComponent<OVRGrabbable>();
	}

	void Start()
	{
		position = transform.position;
		rotation = transform.rotation;
	}	

	void Update()
	{
		if (!grabbable.isGrabbed)
		{
			rb.velocity = Vector3.zero;
			transform.position = Vector3.Lerp(transform.position, position, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
		}
	}

}
