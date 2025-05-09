using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerHealth : MonoBehaviour
{
    [Header("Respawn Settings")]
    public float deathDelay = 1.0f; // Time before respawning
    private Transform[] respawnPoints; // all possible respawn points
    private Transform lastRespawnPoint; 

    [Header("Fall Detection")]
    public float fallThreshold = -10f; // Y position to trigger fall death

    private PlayerController playerController;
    private Rigidbody2D rb;

    private bool isDead = false;

    AudioManager audioManager;

    private Animator animator;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        rb = GetComponent<Rigidbody2D>();

        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();

        animator = GetComponent<Animator>();

        // Find all respawn points
        GameObject respawnParent = GameObject.Find("RespawnPoints");
        if (respawnParent != null)
        {
            List<Transform> points = new List<Transform>();
            foreach (Transform child in respawnParent.transform)
            {
                points.Add(child);
            }
            respawnPoints = points.ToArray();
        }
        else
        {
            Debug.LogError("No RespawnPoints object found!");
        }
    }

    void Update()
    {
        if (!isDead && transform.position.y < fallThreshold)
        {
            Die();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isDead) return;

        if (other.CompareTag("Trap"))
        {
            Die();
        }
    }

    public void Die()
    {
        if (isDead) return;

        isDead = true;
        playerController.DisableMovement();

        // play hurt animation here
        animator.SetBool("hit", true);


        audioManager.PlaySFX(audioManager.death); // plays respawn sound

        FindBestRespawnPoint();
        StartCoroutine(Respawn());
    }

    private void FindBestRespawnPoint()
    {
        float bestDistance = Mathf.Infinity;
        Transform bestPoint = null;
        Vector2 deathPosition = transform.position;

        foreach (var point in respawnPoints)
        {
            // Only consider points behind the player (X coordinate smaller)
            if (point.position.x <= deathPosition.x)
            {
                float distance = Vector2.Distance(deathPosition, point.position);

                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestPoint = point;
                }
            }
        }

        if (bestPoint != null)
        {
            lastRespawnPoint = bestPoint;
        }
        else
        {
            // Fallback: use first respawn point
            lastRespawnPoint = respawnPoints.Length > 0 ? respawnPoints[0] : null;
        }
    }

    public IEnumerator Respawn()
    {
        yield return new WaitForSeconds(deathDelay);

        if (lastRespawnPoint != null)
        {
            Vector3 respawnPos = lastRespawnPoint.position;
            respawnPos.z = 0f; // Lock Z axis
            transform.position = respawnPos;
        }
        else
        {
            Debug.LogWarning("No valid respawn point found!");
        }

        playerController.EnableMovement();
        isDead = false;
    }
}