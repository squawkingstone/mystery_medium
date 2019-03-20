using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggleable : MonoBehaviour 
{	
	public bool toggle_active { get; protected set; }
	public abstract void SetActive(bool active);
}
