using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("▶️ Start button clicked. Loading Game scene...");
        SceneManager.LoadScene("EcoSweep"); // ✅ Must match your scene name exactly
    }

    public void ExitGame()
    {
        Debug.Log("❌ Exit button clicked. Quitting game...");
        Application.Quit();
    }
}
