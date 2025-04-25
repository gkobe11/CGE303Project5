using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPowerUp : MonoBehaviour
{
    private string currentPowerUp = "";
    private bool hasPowerUp = false;

    public KeyCode usePowerUpButton; // set in inspector

    private PlayerController controller;
    public PowerUpUI powerUpUI; // set in inspector

    //Dash power up
    [SerializeField] private float dashSpeed = 30f;
    [SerializeField] private float dashDuration = 0.2f;

    private bool isDashing = false;

    void Start()
    {
        controller = GetComponent<PlayerController>();
    }

    public void ReceivePowerUp(string powerUp)
    {
        currentPowerUp = powerUp;
        hasPowerUp = true;
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
        else if (currentPowerUp == "FireBall")
        {
            StartCoroutine(FireBallRoutine());
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
        isDashing = true;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        PlayerController controller = GetComponent<PlayerController>();

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

    private IEnumerator FireBallRoutine()
    {
        // Implement FireBall logic here
        yield return null; // Placeholder for FireBall logic
    }
}
