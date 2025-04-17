using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpCollection : MonoBehaviour
{
    public string powerUpType = "PlaceHolder";
    // Add more power-up types as needed

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerPowerUp playerPowerUp = other.GetComponent<PlayerPowerUp>();
        if (playerPowerUp != null)
        {
            playerPowerUp.ReceivePowerUp(powerUpType);
            Destroy(gameObject); // Remove the box after pickup
        }
    }
}
