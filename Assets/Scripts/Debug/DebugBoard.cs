using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBoard : MonoBehaviour 
{
	[SerializeField] GameObject debug_menu;


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			debug_menu.SetActive(!debug_menu.activeInHierarchy);
		}
	}
}
