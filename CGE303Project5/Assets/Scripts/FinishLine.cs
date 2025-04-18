using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLine : MonoBehaviour
{
    private bool gameEnded = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            gameEnded = true;
            string winner = other.tag == "Player1" ? "Player 1" : "Player 2";
            GameManager.Instance.ShowWinPanel(winner);
        }
    }
}
