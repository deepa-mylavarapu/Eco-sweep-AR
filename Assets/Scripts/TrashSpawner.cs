using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // ✅ New Input System

public class TrashSpawner : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;

    [Header("Trash Prefabs")]
    public List<GameObject> trashPrefabs;

    [Header("Spawn Settings")]
    public float spawnCooldown = 0.5f;
    public int maxTrashCount = 10;
    public Vector3 trashScale = Vector3.one * 0.3f;

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private int lastIndex = -1;
    private float lastSpawnTime = 0f;
    private int currentTrashCount = 0;

    void Update()
    {
        if (Pointer.current == null || !Pointer.current.press.wasPressedThisFrame) return;
        if (Time.time - lastSpawnTime < spawnCooldown) return;
        lastSpawnTime = Time.time;

        Vector2 touchPosition = Pointer.current.position.ReadValue();
        Debug.Log("📱 Touch detected at: " + touchPosition);

        if (IsTouchOverUI(touchPosition)) return;

        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Pose hitPose = hits[0].pose;
            Debug.Log("✅ Raycast hit at: " + hitPose.position);

            if (trashPrefabs == null || trashPrefabs.Count == 0)
            {
                Debug.LogWarning("⚠️ No trash prefabs assigned!");
                return;
            }

            if (currentTrashCount >= maxTrashCount)
            {
                Debug.Log("🚫 Max trash count reached.");
                return;
            }

            // ✅ Prevent same prefab from spawning twice in a row
            int randomIndex;
            do
            {
                randomIndex = Random.Range(0, trashPrefabs.Count);
            } while (randomIndex == lastIndex && trashPrefabs.Count > 1);
            lastIndex = randomIndex;

            GameObject selectedTrash = trashPrefabs[randomIndex];
            Debug.Log("🗑️ Selected trash prefab: " + selectedTrash.name);

            GameObject spawnedTrash = Instantiate(selectedTrash, hitPose.position, hitPose.rotation);
            spawnedTrash.SetActive(true);
            spawnedTrash.transform.localScale = trashScale;
            currentTrashCount++;

            // ✅ Add Rigidbody and Collider if missing
            if (spawnedTrash.GetComponent<Rigidbody>() == null)
            {
                Rigidbody rb = spawnedTrash.AddComponent<Rigidbody>();
                rb.isKinematic = true;
            }

            if (spawnedTrash.GetComponent<Collider>() == null)
            {
                spawnedTrash.AddComponent<BoxCollider>();
            }

            Debug.Log("✅ Trash spawned at: " + spawnedTrash.transform.position);
        }
        else
        {
            Debug.LogWarning("❌ Raycast did not hit any AR plane.");
        }
    }

    bool IsTouchOverUI(Vector2 touchPosition)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = touchPosition;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0;
    }
}






