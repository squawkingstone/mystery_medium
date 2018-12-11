using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleProp : MonoBehaviour 
{
	/* I'll figure out how to do it so this works with single objects (toggles
	 * them on and off), and switching objects.
	 */

	GameObject obj1 = null;
	GameObject obj2 = null;

	void Start () 
	{
		Debug.Log(name + " : " + transform.childCount);
		if (transform.childCount == 2)
		{
			obj1 = transform.GetChild(0).gameObject;
			obj2 = transform.GetChild(1).gameObject;
			obj1.SetActive(true);
			obj2.SetActive(false);
		}
		else
		{
			obj1 = transform.GetChild(0).gameObject;
			obj1.SetActive(false);
		}
	}

	[ContextMenu("Toggle")]
	public void Toggle()
	{
		bool t = obj1.activeInHierarchy;
		obj1.SetActive(!t);
		if (obj2 != null) obj2.SetActive(t);
	}

	public void SetRevealed(bool revealed)
	{
		obj1.SetActive(!revealed);
		if (obj2 != null) obj2.SetActive(revealed);
	}
}
