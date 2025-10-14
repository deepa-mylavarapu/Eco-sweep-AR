using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem; // ✅ New Input System
using System.Collections.Generic;
using UnityEngine.SceneManagement;


public class TrashInteractor : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;

    private GameObject selectedTrash;
    private bool isDragging = false;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Update()
    {
        // ✅ New Input System: Check for press and movement
        if (Pointer.current == null) return;

        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 touchPosition = Pointer.current.position.ReadValue();
            HandleTouchBegin(touchPosition);
        }
        else if (Pointer.current.press.isPressed && isDragging)
        {
            Vector2 touchPosition = Pointer.current.position.ReadValue();
            HandleTouchMove(touchPosition);
        }
        else if (Pointer.current.press.wasReleasedThisFrame)
        {
            HandleTouchEnd();
        }
    }

    void HandleTouchBegin(Vector2 touchPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.CompareTag("Trash"))
            {
                selectedTrash = hit.collider.gameObject;
                isDragging = true;
                HighlightTrash(selectedTrash);
                Debug.Log($"🗑️ Selected trash: {selectedTrash.name} at {selectedTrash.transform.position}");
            }
            else
            {
                Debug.Log("⚠️ Hit something, but it's not tagged as Trash.");
            }
        }
    }

    void HandleTouchMove(Vector2 touchPosition)
    {
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            selectedTrash.transform.position = hitPose.position;
            Debug.Log($"🚚 Dragging trash to: {hitPose.position}");
        }
    }

    void HandleTouchEnd()
    {
        isDragging = false;
        selectedTrash = null;
        Debug.Log("🛑 Drag ended.");
    }

    void HighlightTrash(GameObject trash)
    {
        Renderer renderer = trash.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.yellow; // Temporary highlight
        }
    }
}

