using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugController : MonoBehaviour 
{
	[System.Serializable]
	private struct DebugConnection
	{
		public Dropdown d1;
		public Dropdown d2;
	}

	[SerializeField] GameObject debug_menu;
	[SerializeField] DebugConnection[] debug_connections;

	[SerializeField] EvidenceModel model;

	void Start()
	{
		foreach (DebugConnection d in debug_connections)
		{
			d.d1.onValueChanged.AddListener((value) => { UpdateModel(); });
			d.d2.onValueChanged.AddListener((value) => { UpdateModel(); });
		}
		debug_menu.SetActive(false);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			debug_menu.SetActive(!debug_menu.activeInHierarchy);
		}
	}

	public void UpdateModel()
	{
		List<Connection<string>> connections = new List<Connection<string>>();
		foreach (DebugConnection c in debug_connections)
		{
			connections.Add(new Connection<string>(c.d1.options[c.d1.value].text, c.d2.options[c.d2.value].text));
		}	
		model.UpdateState(connections);
	}
}
