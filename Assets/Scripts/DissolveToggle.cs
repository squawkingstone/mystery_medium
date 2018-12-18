using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DissolveToggle : MonoBehaviour 
{
	[SerializeField] GameObject[] _objs;
	[SerializeField] float _time;

	Material[] _mats;

	void Awake()
	{
		_mats = new Material[_objs.Length];
		for (int i = 0; i < _mats.Length; i++)
		{
			Renderer r = _objs[i].GetComponent<Renderer>();
			r.sharedMaterial = new Material(r.sharedMaterial);
			_mats[i] = r.sharedMaterial;
		}
		SetThreshold(1f);
	}

	[ContextMenu("FadeIn")]  public void FadeIn() { StartCoroutine(Dissolve(1f, 0f, 1f)); }
	[ContextMenu("FadeOut")] public void FadeOut() { StartCoroutine(Dissolve(0f, 1f, 1f)); }

	void SetThreshold(float value)
	{
		for (int i = 0; i < _mats.Length; i++)
		{
			_mats[i].SetFloat("_Threshold", Mathf.Clamp01(value));
		}
	}

	private IEnumerator Dissolve(float start, float end, float time)
	{
		for (float t = 0f; t < time; t += Time.deltaTime)
		{
			SetThreshold(Mathf.Lerp(start, end, t / time));
			yield return null;
		}
		SetThreshold(end);
	}
}

[CustomEditor(typeof(DissolveToggle))]
class DissolveEditor : Editor
{
	DissolveToggle _toggle;

	void OnEnable() 
	{
		_toggle = (DissolveToggle)target;
	}

	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		if (GUILayout.Button("FadeIn")) { _toggle.FadeIn(); }
		if (GUILayout.Button("FadeOut")) { _toggle.FadeOut(); }
	}
}