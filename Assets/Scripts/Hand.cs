using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Hand : MonoBehaviour 
{	
	[SerializeField] bool left;

	SteamVR_Input_Sources input;

	void Awake()
	{
		input = (left) ? SteamVR_Input_Sources.LeftHand : SteamVR_Input_Sources.RightHand;
	}

	void Update()
	{
		if (SteamVR_Input._default.inActions.Teleport.GetStateDown(input) 
			&& input == SteamVR_Input_Sources.LeftHand)
			Debug.Log("TELEPORT");
		// here, check if I'm intersecting a thing, and then if get state down for the trigger
		// parent the object
	}
}
