using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARSetup : MonoBehaviour
{
    public ARSession arSession;
    public ARPlaneManager arPlaneManager;

    void Start()
    {
        if (arSession == null || arPlaneManager == null)
        {
            Debug.LogError("ARSession or ARPlaneManager not assigned!");
            return;
        }

        arSession.enabled = true;
        arPlaneManager.enabled = true;
        arPlaneManager.requestedDetectionMode = PlaneDetectionMode.Horizontal;
    }
}

