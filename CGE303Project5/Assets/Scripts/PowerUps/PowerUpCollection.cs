using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollection : MonoBehaviour
{
    public List<string> powerUpTypes = new List<string> { "Dash", "FireBall", "Lightning", "Swap" }; // Add more power-up types as needed
    public float respawnTime = 3f; // Time before the power-up box respawns

    private Collider2D collider;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        collider = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPowerUp playerPowerUp = other.GetComponent<PlayerPowerUp>();
        if (playerPowerUp != null && !playerPowerUp.hasPowerUp)
        {
            string randomPowerUp = powerUpTypes[Random.Range(0, powerUpTypes.Count)];
            playerPowerUp.ReceivePowerUp(randomPowerUp);
            StartCoroutine(RespawnRoutine()); // Disable the power-up box for 3 seconds
        }
    }

    private IEnumerator RespawnRoutine()
    {
        collider.enabled = false;
        spriteRenderer.enabled = false;
        yield return new WaitForSeconds(respawnTime);
        collider.enabled = true;
        spriteRenderer.enabled = true;
    }
}
