using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    private string currentPowerUp = "";
    public bool hasPowerUp = false;

    public KeyCode usePowerUpButton; // set in inspector

    private PlayerController controller;
    public PowerUpUI powerUpUI; // set in inspector

    //Dash power up
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.2f;

    //Slow power up
    [SerializeField] private float slowMultiplier = 0.5f;
    [SerializeField] private float slowDuration = 3f;

    //Fireball power up
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private float fireballOffset = 0.5f;
    [SerializeField] private float fireballStunDuration = 1f;

    private bool isDashing = false;

    private Animator animator;

    AudioManager audioManager;

    void Start()
    {
        controller = GetComponent<PlayerController>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        animator = GetComponent<Animator>();
    }

    public void ReceivePowerUp(string powerUp)
    {
        currentPowerUp = powerUp;
        hasPowerUp = true;
        audioManager.PlaySFX(audioManager.collect);
        Debug.Log(gameObject.name + " got power-up: " + powerUp);

        if (powerUpUI != null)
        {   
            powerUpUI.SetPowerUp(powerUp);
        }
    }

    void Update()
    {
        if (hasPowerUp && Input.GetKeyDown(usePowerUpButton))
        {
            ActivatePowerUp();
        }
    }

    void ActivatePowerUp()
    {
        if (currentPowerUp == "Dash")
        {
            StartCoroutine(DashRoutine());

            Debug.Log("Activating Dash Power-Up");
        }
        else if (currentPowerUp == "Slow")
        {
            if (gameObject.CompareTag("Player1"))
                StartCoroutine(SlowPlayer(GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerController>()));
            else if (gameObject.CompareTag("Player2"))
                StartCoroutine(SlowPlayer(GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerController>()));
        }
        else if (currentPowerUp == "FireBall")
        {
            FireBall();
            Debug.Log("Activating FireBall Power-Up");
        }
        // Add more power-ups here as needed

        hasPowerUp = false;
        currentPowerUp = "";

        if (powerUpUI != null)
        {
            powerUpUI.SetPowerUp("");
        }
    }

    private IEnumerator DashRoutine()
    {
        audioManager.PlaySFX(audioManager.dash);

        isDashing = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Get raw player input
        float x = Input.GetAxisRaw(controller.horizontalInput);
        float y = Input.GetAxisRaw(controller.jumpInput);

        Vector2 dashDirection = new Vector2(x, y);

        if (dashDirection.magnitude < 0.1f)
        {
            // Fallback to facing direction if no input
            dashDirection = Vector2.right * (transform.localScale.x >= 0 ? 1 : -1);
        }
        else
        {
            dashDirection = dashDirection.normalized;
        }

        // Disable gravity and movement
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = dashDirection * dashSpeed;

        // Disable PlayerController during dash
        controller.enabled = false;

        yield return new WaitForSeconds(dashDuration);

        // Restore normal movement
        rb.gravityScale = originalGravity;
        rb.velocity = Vector2.zero;
        controller.enabled = true;
        isDashing = false;
    }

    private IEnumerator SlowPlayer(PlayerController target)
    {
        audioManager.PlaySFX(audioManager.zap);

        float originalSpeed = target.moveSpeed;
        float originalJumpForce = target.jumpForce;

        target.moveSpeed *= slowMultiplier;
        target.jumpForce *= slowMultiplier;

        yield return new WaitForSeconds(slowDuration);

        target.moveSpeed = originalSpeed;
        target.jumpForce = originalJumpForce;
    }

    private void FireBall()
    {
        audioManager.PlaySFX(audioManager.fireball);

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Get raw player input
        float x = Input.GetAxisRaw(controller.horizontalInput);
        Vector2 currentPosition = rb.position;

        Vector2 shootDirection = new Vector2(x, 0);
        if (shootDirection.magnitude < 0.1f)
        {
            // Fallback to facing direction if no input
            shootDirection = Vector2.right * (transform.localScale.x >= 0 ? 1 : -1);
        }
        else
        {
            shootDirection = shootDirection.normalized;
        }
        Vector2 spawnPos = currentPosition + shootDirection * fireballOffset;

        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        fireball.GetComponent<FireballProjectile>().Launch(shootDirection, gameObject);
    }

    public void fireballHit(PlayerController player)
    {
        if (player != null)
        {
            animator.SetBool("hit", true);

            StartCoroutine(StunPlayer(player));
        }
    }

    public IEnumerator StunPlayer(PlayerController player)
    {
        audioManager.PlaySFX(audioManager.fireballHit);

        PlayerController enemyController = player.GetComponent<PlayerController>();

        enemyController.DisableMovement();

        // player hurt animation here
        //animator.SetBool("hit", true);

        yield return new WaitForSeconds(fireballStunDuration);
        enemyController.EnableMovement();

        //stop hurt animation
        animator.SetBool("hit", false);
    }
}
