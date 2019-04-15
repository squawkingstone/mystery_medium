using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabbableObjectSnapback : MonoBehaviour 
{
	[SerializeField] Collider _collider;

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
		if (!grabbable.isGrabbed && !IsAtDefaultPos())
		{
			if (_collider.enabled) _collider.enabled = false;
			rb.velocity = Vector3.zero;
			transform.position = Vector3.Lerp(transform.position, position, 0.1f);
			transform.rotation = Quaternion.Lerp(transform.rotation, rotation, 0.1f);
		}
		if (!grabbable.isGrabbed && IsAtDefaultPos() && !_collider.enabled) _collider.enabled = true;
	}

	bool IsAtDefaultPos()
	{
		return (Vector3.Distance(transform.position, position) < 0.0001f);
	}

}
