using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    public float speed = 15f;
    public float stunDuration = 1f;
    private bool hit = false;

    private PlayerController playerController;
    private PlayerPowerUp powerUp;

    private Vector2 direction;

    public void Launch(Vector2 dir, GameObject player)
    {
        playerController = player.GetComponent<PlayerController>();
        powerUp = player.GetComponent<PlayerPowerUp>();
        direction = dir.normalized;
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            PlayerController hitPlayer = collision.GetComponent<PlayerController>();

            if (hitPlayer != null)
            {
                powerUp.fireballHit(hitPlayer);
                Destroy(gameObject);
            }
        }
    }
}
