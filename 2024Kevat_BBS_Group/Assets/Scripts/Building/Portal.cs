using GameState;
using UnityEngine;
// Add this line to access LiveGameTracker

public class Portal : MonoBehaviour
{
    private InGameManager inGameManager;

    private void Awake()
    {
        inGameManager = FindObjectOfType<InGameManager>();
    }

    // checks for collisions with the portal
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the other GameObject has the specific tag
        if (collision.gameObject.CompareTag("Player"))
        {
            // means that the current map has been "won"
            Debug.Log("Collided with GameObject with the TargetTag");
            inGameManager.EndGame(true);
        }
    }
}