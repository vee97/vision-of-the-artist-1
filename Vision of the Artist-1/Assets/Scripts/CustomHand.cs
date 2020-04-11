using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CustomHand : MonoBehaviour
{

	public FingerPinch OnIndexPinch = new FingerPinch();
	public FingerPinch OnMiddlePinch = new FingerPinch();
	public FingerPinch OnRingPinch = new FingerPinch();
	public FingerPinch OnPinkyPinch = new FingerPinch();

	public OVRHand Hand { get; private set; } = null;

	private void Awake()
	{
		Hand = GetComponent<OVRHand>();
	}

    // Update is called once per frame
    private void Update()
    {
     if (Hand.IsSystemGestureInProgress)
     	return;

     if (Hand.GetFingerIsPinching(OVRHand.HandFinger.Index))
     	OnIndexPinch.Invoke(this);

     if (Hand.GetFingerIsPinching(OVRHand.HandFinger.Middle))
     	OnMiddlePinch.Invoke(this);

     if (Hand.GetFingerIsPinching(OVRHand.HandFinger.Ring))
     	OnRingPinch.Invoke(this);

     if (Hand.GetFingerIsPinching(OVRHand.HandFinger.Pinky))
     	OnPinkyPinch.Invoke(this);
    }

    [Serializable]
    public class FingerPinch : UnityEvent<CustomHand> { }
}
