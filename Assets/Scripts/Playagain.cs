using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles menu button actions.
/// </summary>
public class Playagain : MonoBehaviour
{
    /// <summary>
    /// Loads the start screen scene.
    /// </summary>
    public void PlayAgain()
    {
        Debug.Log("Button clicked!");
        SceneManager.LoadScene("Start");
    }
}