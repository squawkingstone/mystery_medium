using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using ConnectionDictionary = System.Collections.Generic.Dictionary<Connection<string>, ModelToggle>;

public class EvidenceView : MonoBehaviour 
{
	[System.Serializable]
	private struct ViewToggle
	{
		public string toggle_string;
		public Toggleable toggle;
	}

	[SerializeField] ViewToggle[] view_toggles;

	[SerializeField] Image win_image;

	Dictionary<string, Toggleable> _toggles;

	void Start()
	{
		_toggles = new Dictionary<string, Toggleable>();
		for (int i = 0; i < view_toggles.Length; i++)
		{
			_toggles.Add(view_toggles[i].toggle_string, view_toggles[i].toggle);
		}
	}

	public void UpdateView(ConnectionDictionary.ValueCollection values, bool win)
	{
		Debug.Log("WIN: " + win);
		foreach (ModelToggle t in values)
		{
			if (t.active != _toggles[t.toggle].toggle_active) _toggles[t.toggle].SetActive(t.active);
		}

		if (win) TriggerWin();
	}	

	[ContextMenu("Trigger Win")]
	void TriggerWin()
	{
		StartCoroutine(WinImageCoroutine());
	}

	IEnumerator WinImageCoroutine()
	{
		for (float t = 0; t < 3f; t += Time.deltaTime)
		{
			win_image.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), new Color(1f, 1f, 1f, 1f), t / 3f);
			yield return null;
		}
		win_image.color = new Color(1f, 1f, 1f, 1f);
	}
}
