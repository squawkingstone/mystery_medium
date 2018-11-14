using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Valve.VR.InteractionSystem;

/* Do some subclassing stuff to do different kinds of interactables, because for the physics
	interactables have diffrent pickup and put down behavior, whereas others dont.
 */

[RequireComponent(typeof(Interactable))]
[RequireComponent(typeof(Rigidbody))]
public class PhysicsInteractable : MonoBehaviour 
{
	UnityAction action = null;
	UnityAction undo = null;
	Interactable interactable;
	Rigidbody rb;

	Hand.AttachmentFlags attachmentFlags = Hand.defaultAttachmentFlags
		| (~Hand.AttachmentFlags.ParentToHand);

	void Awake()
	{
		interactable = GetComponent<Interactable>();
		rb = GetComponent<Rigidbody>();
		// assign some stuff in here
		action = () => {
			transform.localScale = 0.2f * Vector3.one;
		};
		undo = () => {
			transform.localScale = 0.1f * Vector3.one;
		};
	}

	private void HandHoverUpdate( Hand hand )
	{
	    GrabTypes startingGrabType = hand.GetGrabStarting();
        bool isGrabEnding = hand.IsGrabEnding(this.gameObject);

        if (interactable.attachedToHand == null && startingGrabType != GrabTypes.None)
	    {
			rb.isKinematic = true;
            hand.HoverLock(interactable);
            hand.AttachObject(gameObject, startingGrabType, attachmentFlags);
			if (action != null) action();
        }
        else if (isGrabEnding)
        {
            hand.DetachObject(gameObject);
           	hand.HoverUnlock(interactable);
			rb.isKinematic = false;
			rb.velocity = hand.GetTrackedObjectVelocity();
			if (undo != null) undo();
		}
    }	

}
