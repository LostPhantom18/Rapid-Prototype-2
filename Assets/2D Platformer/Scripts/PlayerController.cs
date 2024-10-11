using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Platformer.PlayerController;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

namespace Platformer
{
        public class PlayerController : MonoBehaviour
    {
        public float movingSpeed;
        public float jumpForce;
        private float moveInput;

        private bool facingRight = false;
        [HideInInspector]
        public bool deathState = false;

        private bool isGrounded;
        public Transform groundCheck;

        private Rigidbody2D rigidbody;
        private Animator animator;
        private GameManager gameManager;
        
            void Start()
        {
            rigidbody = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }
        // Detect collisions with enemies
        void OnCollisionEnter2D(Collision2D collision)
        {
            // Assuming enemies have the tag "Enemy"
            if (collision.gameObject.CompareTag("Enemy"))
            {
                // Call the player death logic when hitting an enemy
                PlayerDies();
            }
        }
        public class playercollision : MonoBehaviour
        { }
            private LifeManager lifemanager;
        void PlayerDies()
        {
            // Call the LoseLife function in LifeManager
            lifemanager.LoseLife();

            // Optionally add some player death effects (like animations or sounds)
            Debug.Log("Player lost a heart!");
        }
        // Initial number of hearts (lives)
        public int playerLives = 3;

        // Call this function when player dies or collides with an enemy
        public void LoseLife()
        {
            // Decrease life (heart) by 1
            playerLives--;

            if (playerLives <= 0)
            {
                // Trigger game over if no hearts left
                GameOver();
            }
        }
        // Function for game over (when no lives are left)
        void GameOver()
        {
            // Game Over logic (reload scene, show game over screen, etc.)
            Debug.Log("Game Over!");
            SceneManager.LoadScene("GameOverScene");  // Load a game over scene, if applicable
        }

        private void FixedUpdate()
        {
            CheckGround();
        }

        void Update()
        {
            if (Input.GetButton("Horizontal")) 
            {
                moveInput = Input.GetAxis("Horizontal");
                Vector3 direction = transform.right * moveInput;
                transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, movingSpeed * Time.deltaTime);
                animator.SetInteger("playerState", 1); // Turn on run animation
            }
            else
            {
                if (isGrounded) animator.SetInteger("playerState", 0); // Turn on idle animation
            }
            if(Input.GetKeyDown(KeyCode.Space) && isGrounded )
            {
                rigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
            }
            if (!isGrounded)animator.SetInteger("playerState", 2); // Turn on jump animation

            if(facingRight == false && moveInput > 0)
            {
                Flip();
            }
            else if(facingRight == true && moveInput < 0)
            {
                Flip();
            }
        }

        private void Flip()
        {
            facingRight = !facingRight;
            Vector3 Scaler = transform.localScale;
            Scaler.x *= -1;
            transform.localScale = Scaler;
        }

        private void CheckGround()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.transform.position, 0.2f);
            isGrounded = colliders.Length > 1;
        }

        private void OnTriggerEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Enemy")
            {
                gameManager.heartsLeft -= 1;
                Debug.Log("Hearts left: " + gameManager.heartsLeft);
                if (gameManager.heartsLeft <= 0)
                {
                    deathState = true; // Say to GameManager that player is dead
                }
            }
            else
            {
                deathState = false;
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.tag == "Coin")
            {
                gameManager.coinsCounter += 1;
                Destroy(other.gameObject);
            }
        }
    }
}
