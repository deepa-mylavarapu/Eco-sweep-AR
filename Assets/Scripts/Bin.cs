using UnityEngine;

public class Bin : MonoBehaviour
{
    public TrashType acceptedType;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("🟡 Bin triggered by: " + other.name);
        TrashItem trash = other.GetComponent<TrashItem>();
        if (trash != null)
        {
            if (trash.trashType == acceptedType)
            {
                Debug.Log($"✅ Correctly sorted: {trash.name} into {acceptedType} bin!");

                // ✅ Show educational popup
                LevelManager lm = FindObjectOfType<LevelManager>();
                if (lm != null)
                {
                    lm.ShowInfo(trash.didYouKnowFact);
                }

                // ✅ Update score
                GameManager gm = FindObjectOfType<GameManager>();
                if (gm != null)
                {
                    gm.AddScore(1);
                }

                // ✅ Destroy trash after sorting
                Destroy(trash.gameObject); // or play animation
            }
            else
            {
                Debug.Log($"❌ Incorrect sorting: {trash.name} into {acceptedType} bin.");
            }
        }
    }
}


