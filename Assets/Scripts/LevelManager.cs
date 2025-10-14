using UnityEngine;
using TMPro; // ✅ Import TextMeshPro namespace

public class LevelManager : MonoBehaviour
{
    public GameObject infoPopup;
    public TextMeshProUGUI infoText; // ✅ Use TMP-compatible type

    public void ShowInfo(string message)
    {
        if (infoPopup != null && infoText != null)
        {
            Debug.Log("📢 Showing info: " + message);
            infoText.text = message;
            infoPopup.SetActive(true);
        }

        Debug.Log("ℹ️ InfoPopup shown: " + message);
    }
}
