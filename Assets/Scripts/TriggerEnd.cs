using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerEnd : MonoBehaviour 
{
	[SerializeField] Image fade;

	[ContextMenu("Trigger")]
	public void Trigger()
	{
		StartCoroutine(FadeOut());
	}

	private IEnumerator FadeOut()
	{
		for (float t = 0f; t < 4f; t += Time.deltaTime)
		{
			fade.color = new Color(1f, 1f, 1f, t / 4f);
			yield return null;
		}
		Application.Quit();
	}
}
