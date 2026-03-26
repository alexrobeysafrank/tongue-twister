/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Opens after collecting a set number of keys.
*****************************************************************************/
using UnityEngine;

/// <summary>
/// Handles door unlocking after collecting keys.
/// </summary>
public class Door : MonoBehaviour
{
    [SerializeField] private int keysRequired = 3; // number of keys to open
    private int keysCollected = 0;
    private bool isOpen = false;

    /// <summary>
    /// Increments key count and opens door if enough collected.
    /// </summary>
    public void CollectKey()
    {
        if (isOpen) return;

        keysCollected++;
        Debug.Log("Keys collected: " + keysCollected + "/" + keysRequired);

        if (keysCollected >= keysRequired)
        {
            OpenDoor();
        }
    }

    /// <summary>
    /// Opens door by deactivating it or playing animation.
    /// </summary>
    private void OpenDoor()
    {
        isOpen = true;
        Debug.Log("Door opened!");

       
        gameObject.SetActive(false);
    }
}