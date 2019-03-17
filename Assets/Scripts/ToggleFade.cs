using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleFade : MonoBehaviour 
{
	[SerializeField] List<GameObject> objects;
	[SerializeField] float glow_in_time;
	[SerializeField] float glow_out_time;

	List<Material> _materials;

	void Start()
	{
		_materials = new List<Material>();

		foreach (GameObject g in objects)
		{
			Renderer r = g.GetComponent<Renderer>();
			r.sharedMaterial = new Material(r.sharedMaterial);
			_materials.Add(r.sharedMaterial);
		}
	}

	[ContextMenu("FadeIn")]
	void FadeIn() { StartCoroutine(FadeInRoutine()); }

	[ContextMenu("FadeOut")]
	void FadeOut() { StartCoroutine(FadeOutRoutine()); }

	IEnumerator FadeInRoutine()
	{
		SetFloat("_Glow", 1f);
		SetFloat("_Transparency", 1f);
		for (float t = 0; t < glow_in_time; t += Time.deltaTime)
		{
			SetFloat("_Transparency", 1f - (t / glow_in_time));
			yield return null;
		}
		SetFloat("_Transparency", 0f);
		for (float t = 0; t < glow_out_time; t += Time.deltaTime)
		{
			SetFloat("_Glow", 1f - (t / glow_out_time));
			yield return null;
		}
		SetFloat("_Glow", 0f);
	}

	IEnumerator FadeOutRoutine()
	{
		SetFloat("_Glow", 0f);
		SetFloat("_Transparency", 0f);
		for (float t = 0; t < glow_out_time; t += Time.deltaTime)
		{
			SetFloat("_Glow",(t / glow_out_time));
			yield return null;
		}
		SetFloat("_Glow", 1f);
		for (float t = 0; t < glow_in_time; t += Time.deltaTime)
		{
			SetFloat("_Transparency", (t / glow_in_time));
			yield return null;
		}
		SetFloat("_Transparency", 1f);
		
	}

	void SetFloat(string param, float value)
	{
		foreach (Material m in _materials) { m.SetFloat(param, value); }
	}
}
