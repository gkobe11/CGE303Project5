using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollection : MonoBehaviour
{
    public List<string> powerUpTypes = new List<string> { "Dash", "FireBall", "Lightning", "Swap" }; // Add more power-up types as needed

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPowerUp playerPowerUp = other.GetComponent<PlayerPowerUp>();
        if (playerPowerUp != null && !playerPowerUp.hasPowerUp)
        {
            string randomPowerUp = powerUpTypes[Random.Range(0, powerUpTypes.Count)];
            playerPowerUp.ReceivePowerUp(randomPowerUp);
            Destroy(gameObject); // Remove the box after pickup
        }
    }
}
