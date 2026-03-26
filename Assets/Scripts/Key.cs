/*****************************************************************************
// File Name : Comments.cs
// Author : Alexander R. Safranek
// Creation Date : January 27, 2026
// Last Updated : 3/25/2026 
// Brief Description : Collectible that informs the door when picked up.
*****************************************************************************/
using UnityEngine;

/// <summary>
/// Handles key pickup and informs the linked door.
/// </summary>
public class Key : MonoBehaviour
{
    public Door door; // assign the door this key unlocks

    /// <summary>
    /// Detects player collision and collects key.
    /// </summary>
    /// <param name="other">Collider of object entering trigger</param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Tell the door this key was collected
            door.CollectKey();

            // Remove the key from the scene
            Destroy(gameObject);
        }
    }
}