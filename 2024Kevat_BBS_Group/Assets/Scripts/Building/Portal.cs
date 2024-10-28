using System;
using System.Collections;
using System.Collections.Generic;
using MapSelect;
using GameState; // Add this line to access LiveGameTracker
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    private BuildingManager buildingManager;
    private InGameManager inGameManager;

    private void Awake()
    {
        inGameManager = FindObjectOfType<InGameManager>();
        buildingManager = FindObjectOfType<BuildingManager>();
    }

    // checks for collisions with the portal
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the other GameObject has the specific tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // means that the current map has been "won"
            Debug.Log("Collided with GameObject with the TargetTag");
            inGameManager.PlayerTouchedPortal();
        }
    }
}