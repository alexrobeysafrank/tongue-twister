/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Shows win UI when player touches trophy
*****************************************************************************/
using UnityEngine;
using TMPro;

/// <summary>
/// Handles player touching trophy and winning game.
/// </summary>
public class WinTrigger : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject winUI;           // The win screen panel
    [SerializeField] private TextMeshProUGUI livesText;  // Player lives UI
    [SerializeField] private TextMeshProUGUI timerText;  // Timer UI

    private bool hasWon = false;

    /// <summary>
    /// Detects player entering trigger to win game.
    /// </summary>
    /// <param name="other">Collider of object entering trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (hasWon) return;

        if (other.CompareTag("Player"))
        {
            WinGame();
        }
    }

    /// <summary>
    /// Shows win UI, hides lives/timer, pauses game.
    /// </summary>
    private void WinGame()
    {
        hasWon = true;

        Debug.Log("YOU WIN!");

        if (winUI != null)
            winUI.SetActive(true);

        if (livesText != null)
            livesText.gameObject.SetActive(false);
        if (timerText != null)
            timerText.gameObject.SetActive(false);

        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}