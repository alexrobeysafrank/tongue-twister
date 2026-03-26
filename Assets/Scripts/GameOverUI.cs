/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Handles retry and quit buttons for Game Over.
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles retry and quit functionality from Game Over UI.
/// </summary>
public class GameOverUI : MonoBehaviour
{
    /// <summary>
    /// Reloads current scene to retry game.
    /// </summary>
    public void Retry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Exits application.
    /// </summary>
    public void Quit()
    {
        Application.Quit();
    }
}