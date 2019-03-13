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

	void Awake()
	{
		_view = GetComponent<EvidenceView>();
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

		foreach (ModelToggle t in _connection_dictionary.Values) { t.SetActive(false); }

		foreach (Connection<string> c in _connections)
		{
			if (_connection_dictionary.ContainsKey(c)) { _connection_dictionary[c].SetActive(_connections.Contains(c)); }
		}

		_view.UpdateView(_connection_dictionary.Values);
	}
}

// [CustomEditor(typeof(EvidenceModel))]
// public class EvidenceModelEditor : Editor
// {
// 	SerializedObject model;
// 	List<Connection<string>> connects;
// 	List<string> toggles;
// 	int size;

// 	void OnEnable()
// 	{
// 		model = serializedObject;

// 		connects = model.FindProperty("valid_connections") as System.Object as List<Connection<string>>;
// 		if (connects == null)
// 		{
// 			connects = new List<Connection<string>>();
// 		}

// 		toggles = model.FindProperty("connections_toggleable") as System.Object as List<string>;
// 		if (toggles == null)
// 		{
// 			toggles = new List<string>();
// 		}
// 	}

// 	public override void OnInspectorGUI()
// 	{
// 		serializedObject.Update();
// 		for (int i = 0; i < connects.Count; i++)
// 		{
// 			EditorGUILayout.BeginHorizontal();

// 			connects[i].first  = EditorGUILayout.TextField(connects[i].first);
// 			connects[i].second = EditorGUILayout.TextField(connects[i].second);
// 			toggles[i] = EditorGUILayout.TextField(toggles[i]);

// 			if (GUILayout.Button("X"))
// 			{
// 				connects.RemoveAt(i);
// 				toggles.RemoveAt(i);
// 				return;
// 			}

// 			EditorGUILayout.EndHorizontal();
// 		}
		
// 		if (GUILayout.Button("Add New Connection"))
// 		{
// 			connects.Add(new Connection<string>("",""));
// 			toggles.Add("");
// 		}
// 	}
// }
