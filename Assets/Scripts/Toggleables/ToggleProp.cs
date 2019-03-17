using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleProp : Toggleable
{
    [SerializeField] GameObject[] objects;

	private struct PropTransform
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;
	} 

	PropTransform[] prop_transforms;

	void Start()
	{
		prop_transforms = new PropTransform[objects.Length];
		for (int i = 0; i < objects.Length; i++)
		{
			prop_transforms[i].position = objects[i].transform.localPosition;
			prop_transforms[i].rotation = objects[i].transform.localRotation;
			prop_transforms[i].scale    = objects[i].transform.localScale;
		}
		this.active = true;
		SetActive(false);
	}

    public override void SetActive(bool active)
    {
		if (this.active != active)
		{
			for (int i = 0; i < objects.Length; i++)
			{
				objects[i].SetActive(active);
			}
		
			// If we're activating them, then reset the position
			if (active)
			{
				for (int i = 0; i < objects.Length; i++)
				{
					objects[i].transform.localPosition = prop_transforms[i].position;
					objects[i].transform.localRotation = prop_transforms[i].rotation;
					objects[i].transform.localScale    = prop_transforms[i].scale;
				}
			}
		}

		this.active = active;
    }

	[ContextMenu("Toggle On")]
	void ToggleOn() { SetActive(true); }

	[ContextMenu("Toggle Off")]
	void ToggleOff() { SetActive(false); }
}
