using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleProp : Toggleable
{
    [SerializeField] GameObject[] objects;
	[SerializeField] GameObject sound_object;

	private struct PropTransform
	{
		public Vector3 position;
		public Quaternion rotation;
		public Vector3 scale;
	} 

	private struct Display
	{
		public Renderer renderer;
		public MaterialPropertyBlock block;
	}

	PropTransform[] prop_transforms;
	List<Display> materials;

	AudioSource sound;

	void Start()
	{

		prop_transforms = new PropTransform[objects.Length];
		materials = new List<Display>();
		for (int i = 0; i < objects.Length; i++)
		{
			prop_transforms[i].position = objects[i].transform.localPosition;
			prop_transforms[i].rotation = objects[i].transform.localRotation;
			prop_transforms[i].scale    = objects[i].transform.localScale;

			Display d;

			Renderer r = objects[i].GetComponent<Renderer>();
			// for (int j = 0; j < r.sharedMaterials.Length; j++)
			// {
			// 	// Material m = new Material(r.sharedMaterials[j]);
			// 	// r.SetPropertyBlock(r.GetPropertyBlck())
			// 	// materials.Add(r.sharedMaterials[j])o;
			// 	materials.
			// }

			d.renderer = r;
			d.block = new MaterialPropertyBlock();

			materials.Add(d);

		}
		sound = sound_object.GetComponent<AudioSource>();
		SetMatProperty("_Revealed", 0f);
		toggle_active = false;
	}
	
	public override void SetActive(bool active)
	{
		if (active)
		{
			sound.Play();
			Activate();
			toggle_active = true;
		}
		else
		{
			Deactivate();
			toggle_active = false;
		}
	}

	void Activate() { StartCoroutine(AnimateReveal(0f, 1f)); }
	void Deactivate() { StartCoroutine(AnimateReveal(1f, 0f)); }

	IEnumerator AnimateReveal(float start, float end)
	{
		for (float t = 0; t < 2f; t += Time.deltaTime)
		{
			SetMatProperty("_Revealed", Mathf.Lerp(start, end, t / 2f));
			yield return null;
		}
	} 

	void SetMatProperty(string property, float value)
	{
		for (int i = 0; i < materials.Count; i++)
		{
			materials[i].renderer.GetPropertyBlock(materials[i].block);
			materials[i].block.SetFloat(property, value);
			materials[i].renderer.SetPropertyBlock(materials[i].block);
		}
	}

	[ContextMenu("Toggle On")]
	void ToggleOn() { SetActive(true); }

	[ContextMenu("Toggle Off")]
	void ToggleOff() { Deactivate(); }
}
