using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class OclusionManager : MonoBehaviour
{
    public AROcclusionManager occlusionManager;

    void Start()
    {
        occlusionManager.enabled = GameManager.OclusionActiva;
    }
}
