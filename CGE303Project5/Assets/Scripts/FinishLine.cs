using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishLine : MonoBehaviour
{
    private bool gameEnded = false;

    AudioManager audioManager; // reference to the AudioManager script

    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (gameEnded) return;

        if (other.CompareTag("Player1") || other.CompareTag("Player2"))
        {
            audioManager.PlaySFX(audioManager.win); // victory sound

            gameEnded = true;
            string winner = other.tag == "Player1" ? "Player 1" : "Player 2";
            GameManager.Instance.ShowWinPanel(winner);
            UnlockNewLevel(); // Unlocks next level
        }
    }

    // Unlock next level function
    void UnlockNewLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex >= PlayerPrefs.GetInt("ReachedIndex"))
        {
            PlayerPrefs.SetInt("ReachedIndex", SceneManager.GetActiveScene().buildIndex + 1);
            PlayerPrefs.SetInt("UnlockedLevel", PlayerPrefs.GetInt("UnlockedLevel1", 1) + 1);
            PlayerPrefs.Save();
        }
    }
}
