// Credits to Dilmer Valecillos for VRDraw.cs tutorial and code.

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OculusSampleFramework;
using static OVRHand;


public enum HandToTrack2
	{
		Left,
		Right
	}

public class DrawLine : MonoBehaviour
{

	[SerializeField]
	// set Left to be default initially
	private HandToTrack2 handToTrack2 = HandToTrack2.Left;

	[SerializeField]
	// gameobject used to track movement
	// either tracking bone for the right or left hand
	private GameObject objectToTrackMovement;

	[SerializeField, Range (0, 1.0f)]
	private float minDistanceBeforeNewPoint = 0.2f;

	// need to calculate previous point distance versus new point distance
	// if those two are greater than the minimum, then create new point in line renderer
	private Vector3 prevPointDistance = Vector3.zero;

	[SerializeField]
	// if oculus says pinch is above or equal to 0.5f, then know to start drawing.
	private float minFingerPinchStrength = 0.5f;

	[SerializeField, Range(0, 1.0f)]
	private float lineDefaultWidth = 0.010f;

	private int positionCount = 0;
	// line renderer created out of Vector3 points
	// need to keep track of how many points are created bc line renderer needs to know

	// keep track of all of the line renderers that are created
	// At some point, will be destroyed
	private List<LineRenderer> lines = new List <LineRenderer>();

	// keep track of the current line renderer
	private LineRenderer currentLineRender;

	// set white to be default colour
	[SerializeField]
	private Color defaultColor = Color.white;

	[SerializeField]
	// another gameobject for the editor
	// so if want to debug in editor, have another game object for editor
	private GameObject editorObjectToTrackMovement;

	[SerializeField]
	private bool allowEditorControls = true;

	[SerializeField]
	// default material used for line renderer
	private Material defaultLineMaterial;

	// bool to determine if pinching down
	// as soon as a pinch is detected, need to know when it is held and released
	private bool IsPinchingReleased = false;

	// everything related to Oculus will be here
	#region Oculus Types

		private OVRHand ovrHand;

		// can query the hand skeleton
		private OVRSkeleton ovrSkeleton;

		// can get info about bones
		private OVRBone boneToTrack;
	#endregion

	void Awake()
	{
		// get reference to ovrHand and ovrSkeleton
		ovrHand = objectToTrackMovement.GetComponent<OVRHand>();
		ovrSkeleton = objectToTrackMovement.GetComponent<OVRSkeleton>();

		// unity editor debugging purposes
		#if UNITY_EDITOR

		if(allowEditorControls)
		{
			objectToTrackMovement = editorObjectToTrackMovement != null ? editorObjectToTrackMovement : objectToTrackMovement;
		}

		#endif

		// get initial bone to track
		// bone to track will be Hand_Index1 - can make this a serializable option
		// at some point in future, can tell system which finger we'll use for drawing
		// index finger works well for now
		boneToTrack = ovrSkeleton.Bones
			.Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index1)
			.SingleOrDefault();

		// add initial line renderer
		AddNewLineRenderer();
	}

	// create new lineRenderer as soon as awake function gets executed
	void AddNewLineRenderer()
	{
		// set initial positionCount variable to 0
		positionCount = 0;

		// create gameobject
		GameObject go = new GameObject($"LineRenderer_{handToTrack2.ToString()}_{lines.Count}");
		
		// set parent & position
		go.transform.parent = objectToTrackMovement.transform.parent;
		go.transform.position = objectToTrackMovement.transform.position;

		// add linerenderer to that game object
		LineRenderer goLineRenderer = go.AddComponent<LineRenderer>();

		// set start and end width
		goLineRenderer.startWidth = lineDefaultWidth;
		goLineRenderer.endWidth = lineDefaultWidth;

		// tell it to use world space
		goLineRenderer.useWorldSpace = true;
		goLineRenderer.material = defaultLineMaterial;
		goLineRenderer.positionCount = 1;
		goLineRenderer.numCapVertices = 5;

		// set new lineRenderer to currentlinerender variable
		currentLineRender = goLineRenderer;

		// add lineRenderer to list of lines
		lines.Add(goLineRenderer);

	}


    // Update is called once per frame
    void Update()
    {
    	// if boneToTrack hasn't been set, going to try again
        if (boneToTrack == null)
        {
        	boneToTrack = ovrSkeleton.Bones
	        	.Where(b => b.Id == OVRSkeleton.BoneId.Hand_Index3)
	        	.SingleOrDefault();

	        objectToTrackMovement = boneToTrack.Transform.gameObject;
        }

        // call function to check for pinch stake
        CheckPinchState();
    }

    private void CheckPinchState()
    {
    	// ovrHand getfingerispinching, pass in finger I want to detect (index in this case)
    	bool isIndexFingerPinching = ovrHand.GetFingerIsPinching(OVRHand.HandFinger.Index);

    	// determine finger pinch strength 
    	// can pass in different fingers
    	float indexFingerPinchStrength = ovrHand.GetFingerPinchStrength(OVRHand.HandFinger.Index);

    	// if pinching and strength is greater than or equal to min set in serializable fields
    	// then update the line
    	// otherwise, create new line renderer (drawing new line)
    	if (isIndexFingerPinching && indexFingerPinchStrength >= minFingerPinchStrength)

    	{
    		// (If pinching continuously - keep updating current line)
    		UpdateLine();
    		IsPinchingReleased = true; // set to false by default
    	}

    	// as soon as I let go of pinching, fall into else
    	else
    	{
	    	// if isPinchingDown is not true 
	    	// if false will create new line renderer
    		// if (!IsPinchingDown) old version
    		if (IsPinchingReleased)
    		{
    			AddNewLineRenderer();
    			IsPinchingReleased = false;
    		}
    	}
    }

    void UpdateLine()
    {
    	// if prevPointDistance is null
    	// track prevPointDistance of object I'm tracking (in this case it is the bone)
    	if(prevPointDistance == null)
    	{
    		prevPointDistance = objectToTrackMovement.transform.position;
    	}

    	// if prevPointDistance is NOT null and distance b/w prevpoint + position of object.transform.position
    	// is greater than minDistanceBeforeNewPoint
    	// then know it is within threshold set w/in public variables
    	if(prevPointDistance != null && Mathf.Abs(Vector3.Distance(prevPointDistance, objectToTrackMovement.transform.position)) >= minDistanceBeforeNewPoint)
    	{
    		// get direction
    		// get new prevPointDistance
    		// Add new point
    		Vector3 dir = (objectToTrackMovement.transform.position - Camera.main.transform.position).normalized;
    		prevPointDistance = objectToTrackMovement.transform.position;
    		AddPoint(prevPointDistance, dir);
    	}
    }

    void AddPoint(Vector3 position, Vector3 direction)
    {
    	// pass in positionCount and position tracked
    	// increment position count for variable above
    	// increment positionCount in currentLineRender for actual lineRenderer
    	// set that position
    	currentLineRender.SetPosition(positionCount, position);
    	positionCount++;
    	currentLineRender.positionCount = positionCount + 1;
    	currentLineRender.SetPosition(positionCount, position);
    }

    // not currently used
    public void UpdateLineWidth(float newValue)
    {
    	currentLineRender.startWidth = newValue;
    	currentLineRender.endWidth = newValue;
    	lineDefaultWidth = newValue;
    }

    public void UpdateLineColor(Color color)
    {
    	// in case we haven't drawn anything
    	if(currentLineRender.positionCount == 1)
    	{
    		currentLineRender.material.color = color;
    		currentLineRender.material.EnableKeyword("_EMISSION");
    		currentLineRender.material.SetColor("_EmissionColor", color);
    	}
    	defaultColor = color;
    	defaultLineMaterial.color = color;
    }

    public void UpdateLineMaterial(Material material)
    {
		defaultLineMaterial = material;
	}

    public void UpdateMinDistance(float newValue)
    {
    	// will draw more points if smaller line - more smooth line
    	minDistanceBeforeNewPoint = newValue;
    }
}

