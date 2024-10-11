using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine;
using UnityEngine.SceneManagement;  // To reload the scene or go to a game-over scene

public class LifeManager : MonoBehaviour
{
    public int playerLives = 3;  // Initial number of hearts (lives)

    // Call this function when player dies or collides with an enemy
    public void LoseLife()
    {
        playerLives--;  // Decrease life (heart) by 1

        if (playerLives <= 0)
        {
            GameOver();  // Trigger game over if no hearts left
        }
    }

    // Function for game over (when no lives are left)
    void GameOver()
    {
        // Game Over logic (reload scene, show game over screen, etc.)
        Debug.Log("Game Over!");
        SceneManager.LoadScene("GameOverScene");  // Load a game over scene, if applicable
    }
}

public class PlayerCollision : MonoBehaviour
{
    private LifeManager lifeManager;

    void Start()
    {
        lifeManager = FindObjectOfType<LifeManager>();  // Reference the LifeManager script
    }

    // Detect collisions with enemies
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))  // Assuming enemies have the tag "Enemy"
        {
            PlayerDies();  // Call the player death logic when hitting an enemy
        }
    }

    void PlayerDies()
    {
        lifeManager.LoseLife();  // Call the LoseLife function in LifeManager

        // Optionally add some player death effects (like animations or sounds)
        Debug.Log("Player lost a heart!");
    }
}
