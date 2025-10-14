using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.InputSystem; // ✅ New Input System
using System.Collections.Generic;

public class BinSpawner : MonoBehaviour
{
    [Header("AR Components")]
    public ARRaycastManager raycastManager;

    [Header("Bin Prefabs")]
    public GameObject[] binPrefabs; // Assign 4 bin prefabs in Inspector

    [Header("Spawn Settings")]
    [Tooltip("Horizontal spacing between bins in meters")]
    public float spacing = 0.3f; // Adjust for more room between bins

    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private bool binsSpawned = false;

    void Update()
    {
        // ✅ New Input System: Detect press this frame
        if (binsSpawned || Pointer.current == null || !Pointer.current.press.wasPressedThisFrame) return;

        Vector2 touchPosition = Pointer.current.position.ReadValue();
        Debug.Log("📱 Touch detected at: " + touchPosition);

        if (!raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            Debug.LogWarning("❌ Raycast missed — no AR plane detected.");
            return;
        }

        Pose hitPose = hits[0].pose;
        Debug.Log("✅ Raycast hit at: " + hitPose.position);

        if (binPrefabs == null || binPrefabs.Length == 0)
        {
            Debug.LogWarning("❌ No bin prefabs assigned in Inspector.");
            return;
        }

        SpawnBins(hitPose);
        binsSpawned = true;
        Debug.Log("✅ All bins spawned successfully.");
    }

    void SpawnBins(Pose centerPose)
    {
        float centerOffset = (binPrefabs.Length - 1) / 2f;

        for (int i = 0; i < binPrefabs.Length; i++)
        {
            GameObject binPrefab = binPrefabs[i];
            if (binPrefab == null)
            {
                Debug.LogWarning($"⚠️ Bin prefab at index {i} is null.");
                continue;
            }

            // Calculate horizontal offset
            Vector3 offset = new Vector3((i - centerOffset) * spacing, 0, 0);
            Vector3 spawnPosition = centerPose.position + centerPose.rotation * offset;

            GameObject spawnedBin = Instantiate(binPrefab, spawnPosition, centerPose.rotation);
            spawnedBin.SetActive(true);

            // ✅ Scale the bin down for AR visibility
            spawnedBin.transform.localScale = Vector3.one * 0.3f;

            Debug.Log($"🗑️ Spawned bin: {spawnedBin.name} at {spawnPosition}");
        }
    }
}


