using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.SceneManagement;

public class ARSceneController : MonoBehaviour
{
    public ARRaycastManager raycastManager;
    public GameObject trashPrefab;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private int trashCount = 0;
    public int trashGoal = 5;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Vector2 touchPosition = Input.GetTouch(0).position;

            if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                Instantiate(trashPrefab, hitPose.position, hitPose.rotation);
                trashCount++;

                if (trashCount >= trashGoal)
                {
                    SceneManager.LoadScene("LevelComplete");
                }
            }
        }
    }
}

