using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class PlaneClassifier : MonoBehaviour
{
    public List<Pose> horizontalPlaneHits = new List<Pose>();
    public List<Pose> verticalPlaneHits = new List<Pose>();

    private ARRaycastManager raycastManager;
    private static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
    }

    void Update()
    {
        if (Input.touchCount == 0)
            return;

        var touch = Input.GetTouch(0);
        if (touch.phase != TouchPhase.Began)
            return;

        Vector2 touchPosition = touch.position;

        // Horizontal
        if (raycastManager.Raycast(touchPosition, s_Hits, TrackableType.PlaneWithinPolygon))
        {
            foreach (var hit in s_Hits)
            {
                TrackableId planeId = hit.trackableId;
                ARPlane plane = ARPlaneManagerExtensions.GetPlane(raycastManager, planeId);

                if (plane.alignment == PlaneAlignment.HorizontalUp || plane.alignment == PlaneAlignment.HorizontalDown)
                {
                    horizontalPlaneHits.Add(hit.pose);
                    Debug.Log("Horizontal plane detected at: " + hit.pose.position);
                }
                else if (plane.alignment == PlaneAlignment.Vertical)
                {
                    verticalPlaneHits.Add(hit.pose);
                    Debug.Log("Vertical plane detected at: " + hit.pose.position);
                }
            }
        }
    }
}
