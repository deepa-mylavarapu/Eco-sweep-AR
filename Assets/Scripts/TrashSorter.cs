using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TrashSorter : MonoBehaviour
{
    public GameObject infoPopup;
    public Text infoText;
    public float binProximityThreshold = 0.5f;

    void Update()
    {
        GameObject[] trashItems = GameObject.FindGameObjectsWithTag("Trash");

        foreach (GameObject trash in trashItems)
        {
            TrashItem item = trash.GetComponent<TrashItem>();
            if (item == null) continue;

            GameObject targetBin = FindBinForType(item.trashType);
            if (targetBin != null)
            {
                float distance = Vector3.Distance(trash.transform.position, targetBin.transform.position);
                Debug.Log($"🔍 Comparing {item.trashType} with bin tag {targetBin.tag} — Distance: {distance}");

                if (distance < binProximityThreshold)
                {
                    Debug.Log($"✅ Sorted {trash.name} into {targetBin.tag} bin!");
                    Destroy(trash);

                    // ✅ Update score via GameManager
                    GameManager gm = FindObjectOfType<GameManager>();
                    if (gm != null)
                    {
                        gm.AddScore(1);
                    }

                    StartCoroutine(ShowInfo(item.didYouKnowFact));
                }
            }
        }
    }

    GameObject FindBinForType(TrashType type)
    {
        switch (type)
        {
            case TrashType.Plastic: return GameObject.FindWithTag("PlasticBin");
            case TrashType.Metal: return GameObject.FindWithTag("MetalBin");
            case TrashType.Paper: return GameObject.FindWithTag("PaperBin");
            case TrashType.Glass: return GameObject.FindWithTag("GlassBin");
            default: return null;
        }
    }

    IEnumerator ShowInfo(string fact)
    {
        Debug.Log("📘 Showing fact: " + fact);
        infoText.text = fact;
        infoPopup.SetActive(true);
        yield return new WaitForSeconds(3f);
        infoPopup.SetActive(false);
    }
}




