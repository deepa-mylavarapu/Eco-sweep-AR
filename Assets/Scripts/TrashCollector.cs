using UnityEngine;

public class TrashCollector : MonoBehaviour
{
    public GameObject infoPopup;
    public int score = 0;

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Trash"))
                {
                    GameObject trash = hit.collider.gameObject;

                    // Check if trash is near a bin
                    GameObject bin = GameObject.FindWithTag("Bin");
                    if (bin != null && Vector3.Distance(trash.transform.position, bin.transform.position) < 0.5f)
                    {
                        Destroy(trash);
                        score++;
                        infoPopup.SetActive(true);
                    }
                }
            }
        }
    }
}

