using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class RaceCountdown : MonoBehaviour
{
    public Text countdownText; // Assign in Inspector
    public float countdownTime = 3f;

    public PlayerController[] players; // Assign both players here

    void Start()
    {
        StartCoroutine(CountdownRoutine());
    }

    IEnumerator CountdownRoutine()
    {
        // Disable movement for all players
        foreach (var player in players)
            player.enabled = false;

        int count = Mathf.CeilToInt(countdownTime);

        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);

        countdownText.text = "";

        // Enable movement
        foreach (var player in players)
            player.enabled = true;
    }
}