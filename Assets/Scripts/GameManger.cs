using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int targetScore = 10;
    public TextMeshProUGUI scoreText;
    public string finalSceneName = "LevelComplete";

    public void AddScore(int amount)
    {
        score += amount;
        Debug.Log("🏆 Score updated to: " + score);

        if (scoreText != null)
        {
            scoreText.text = "Score: " + score;
        }
        else
        {
            Debug.LogWarning("⚠️ scoreText is not assigned!");
        }

        if (score >= targetScore)
        {
            Debug.Log("🎯 Target score reached. Loading LevelComplete scene...");
            StartCoroutine(LoadLevelCompleteAfterDelay());
        }
    }

    IEnumerator LoadLevelCompleteAfterDelay()
    {
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(finalSceneName);
    }

    // ✅ Manual test: press L to load LevelComplete scene
    void Update()
    {
        if (Keyboard.current.lKey.wasPressedThisFrame)
        {
            SceneManager.LoadScene("LevelComplete");
        }
    }
}





