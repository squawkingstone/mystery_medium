using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
		foreach (ModelToggle t in values)
		{
			_toggles[t.toggle].SetActive(t.active);
		}

		if (win) TriggerWin();
	}	

	void TriggerWin()
	{
		
	}
}
