using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles menu button actions.
/// </summary>
public class Playagain : MonoBehaviour
{
    [SerializeField] private string startSceneName = "Start";

    /// <summary>
    /// Loads the start screen scene.
    /// </summary>
    public void PlayAgain()
    {
        Debug.Log("Play Again clicked");
        SceneManager.LoadScene(startSceneName);
    }
}