using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Toggleable : MonoBehaviour 
{	
	protected bool active;
	public abstract void SetActive(bool active);
}
