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
        if (currentPowerUp == "PlaceHolder")
        {
            StartCoroutine(PlaceHolderRoutine());
        }
        // Add more power-ups here as needed

        hasPowerUp = false;
        currentPowerUp = "";

        if (powerUpUI != null)
        {
            powerUpUI.SetPowerUp("");
        }
    }

    private System.Collections.IEnumerator PlaceHolderRoutine()
    {
        Debug.Log(gameObject.name + " activated Power Up!");

        yield return new WaitForSeconds(3f);

        Debug.Log("Power up ended.");
    }
}
