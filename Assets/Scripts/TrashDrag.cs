using UnityEngine;
using UnityEngine.InputSystem;

public class TrashDrag : MonoBehaviour
{
    private Vector3 offset;
    private Camera arCamera;
    private bool isDragging = false;

    [Header("Bin Detection")]
    public float binDetectionRadius = 0.2f;
    public string binTag = "Bin";

    void Start()
    {
        arCamera = Camera.main;
    }

    void Update()
    {
        if (Pointer.current == null) return;

        // Start drag
        if (Pointer.current.press.wasPressedThisFrame)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            Ray ray = arCamera.ScreenPointToRay(screenPos);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    float z = arCamera.WorldToScreenPoint(transform.position).z;
                    Vector3 worldTouch = arCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
                    offset = transform.position - worldTouch;
                    isDragging = true;
                }
            }
        }

        // Continue drag
        else if (Pointer.current.press.isPressed && isDragging)
        {
            Vector2 screenPos = Pointer.current.position.ReadValue();
            float z = arCamera.WorldToScreenPoint(transform.position).z;
            Vector3 worldTouch = arCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, z));
            transform.position = worldTouch + offset;
        }

        // End drag and check for bin
        else if (Pointer.current.press.wasReleasedThisFrame && isDragging)
        {
            isDragging = false;

            Collider[] nearbyBins = Physics.OverlapSphere(transform.position, binDetectionRadius);
            foreach (Collider bin in nearbyBins)
            {
                if (bin.CompareTag(binTag))
                {
                    StartCoroutine(HandleTrashDrop(bin));
                    return;
                }
            }
        }
    }

    private System.Collections.IEnumerator HandleTrashDrop(Collider bin)
    {
        transform.position = bin.transform.position;

        yield return new WaitForSeconds(0.2f); // Optional delay

        // ✅ Add score via GameManager
        GameManager gm = FindObjectOfType<GameManager>();
        if (gm != null)
        {
            gm.AddScore(1);
        }
        else
        {
            Debug.LogWarning("⚠️ GameManager not found in scene!");
        }

        Destroy(gameObject);
    }
}








