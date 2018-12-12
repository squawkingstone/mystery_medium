using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class ToggleProp : MonoBehaviour 
{
	[SerializeField] Resettable[] objs;

	void Start () 
	{
		SetActive(objs, false);
	}

	[ContextMenu("Enable")]
	void Enable() { SetRevealed(true); }
	[ContextMenu("Disable")]
	void Disable() { SetRevealed(false); }

	public void SetRevealed(bool revealed)
	{
		SetActive(objs, revealed);
		Debug.Log(name + " " + ( (revealed) ? "revealed!" : "hidden!" ));
	}

	public void SetActive(Resettable[] objs, bool active)
	{
		for (int i = 0; i < objs.Length; i++)
		{
			Interactable interact = objs[i].gameObject.GetComponent<Interactable>();
			if (interact != null && interact.attachedToHand != null) 
			{ 
				interact.attachedToHand.DetachObject(interact.gameObject); 
			
			}
			objs[i].ResetTransform();
			objs[i].gameObject.SetActive(active);
		}
	}
}
