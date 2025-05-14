using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public static class ARPlaneManagerExtensions
{
    public static ARPlane GetPlane(this ARRaycastManager raycastManager, TrackableId trackableId)
    {
        var planeManager = GameObject.FindObjectOfType<ARPlaneManager>();
        if (planeManager != null && planeManager.trackables != null)
        {
            foreach (var plane in planeManager.trackables)
            {
                if (plane.trackableId == trackableId)
                    return plane;
            }
        }
        return null;
    }
}
