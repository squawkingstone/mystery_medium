using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using ConnectionDictionary = System.Collections.Generic.Dictionary<Connection<string>, ModelToggle>;

[System.Serializable]
public class Connection<T>
{
	public T first  { get; private set; }
	public T second { get; private set; }

	public Connection(T first, T second)
	{
		this.first = first;
		this.second = second;
	}

	public override bool Equals(object obj)
	{
		Connection<T> c = obj as Connection<T>;
		return (first.Equals(c.first) && second.Equals(c.second)) || 
			   (first.Equals(c.second) && second.Equals(c.first));
	}

	// hash code so we treat stuff with the same vars as the same, should work, but idk
	public override int GetHashCode()
	{
		int prime1 = 47;
		return (prime1 * first.GetHashCode()) + (prime1 * second.GetHashCode());
	}
}

public class ModelToggle
{
	public string toggle { get; private set; }
	public bool active   { get; private set; }

	public ModelToggle(string toggle)
	{
		this.toggle = toggle;
		active = false;
	}

	public void SetActive(bool active) { this.active = active; }
}

public class EvidenceModel : MonoBehaviour 
{
	// View component
	EvidenceView _view;

	// internal models...
	List<Connection<string>> _connections;
	ConnectionDictionary _connection_dictionary;

	// Stuff to "Serialize" the dictionary
	[System.Serializable]
	public struct ConnectDictObj
	{
		public string first;
		public string second;
		public string toggle;
	}

	public ConnectDictObj[] connect_dict;
	[SerializeField] List<string> win_state;

	AudioSource buzzzzzzzz;

	void Awake()
	{
		_view = GetComponent<EvidenceView>();
		buzzzzzzzz = GetComponent<AudioSource>();
	}

	void Start()
	{
		_connection_dictionary = new ConnectionDictionary();
		for (int i = 0; i < connect_dict.Length; i++)
		{
			_connection_dictionary.Add(
				new Connection<string>(connect_dict[i].first, connect_dict[i].second), 
				new ModelToggle(connect_dict[i].toggle));
		}

		foreach (Connection<string> connection in _connection_dictionary.Keys)
		{
			Debug.Log(connection.first + " + " + connection.second + " = " + _connection_dictionary[connection].toggle);
		}

		Debug.Log(_connection_dictionary.ContainsKey(new Connection<string>("CCC","DDD")));
		Debug.Log(_connection_dictionary.ContainsKey(new Connection<string>("DDD","CCC")));
	}

	public void UpdateState(List<Connection<string>> connections)
	{
		_connections = connections;

		List<string> evidence_list = new List<string>();
		foreach (Connection<string> c in _connections)
		{
			evidence_list.Add(c.first);
			evidence_list.Add(c.second);

			//Debug.Log(c.first);
			//Debug.Log(c.second);
		}

		bool win = true;
		foreach (string e in win_state)
		{
			if (!evidence_list.Contains(e)) { win = false; }
		}

		if (evidence_list.Contains("SOLVED") && !win)
		{
			// trigger the you fucked up sound
			buzzzzzzzz.Play();
		}
	
		foreach (ModelToggle t in _connection_dictionary.Values) { t.SetActive(false); }

		foreach (Connection<string> c in _connections)
		{
			if (_connection_dictionary.ContainsKey(c)) { _connection_dictionary[c].SetActive(_connections.Contains(c)); }
		}

		_view.UpdateView(_connection_dictionary.Values, win);
	}
}
