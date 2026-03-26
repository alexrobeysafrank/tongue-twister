/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Handles starting or quitting the game from the start screen
*****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles starting game and quitting application.
/// </summary>
public class StartScreen : MonoBehaviour
{
    /// <summary>
    /// Loads first game scene.
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Level 1");
    }

    /// <summary>
    /// Quits game and logs message in editor.
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }
}