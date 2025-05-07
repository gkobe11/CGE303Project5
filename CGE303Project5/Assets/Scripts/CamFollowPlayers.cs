using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayers : MonoBehaviour
{
    public Transform player1;
    public Transform player2;
    public float smoothing = 5f;
    private float zoomSpeed = 10f;
    public float maxZoomOut = 10f;
    public float minZoomIn = 5f;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        offset = transform.position - (player1.position + player2.position) / 2;
    }

    // Update is called once per frame
    void LateUpdate()
    {
       if(player1 != null && player2 != null)
        {
            //calculate midpoint between the players
            Vector3 midpoint = (player1.position + player2.position) / 2;

            //calculate distance between the players (max 35)
            float distance = Vector3.Distance(player1.position, player2.position);

            //smoothly move camera to midpoint
            Vector3 targetPosition = midpoint + offset;
            targetPosition.z = transform.position.z; // Lock Z position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothing * Time.deltaTime);

            //adjust camera's zoom based on distance between players
            Camera camera = GetComponent<Camera>();
            if(camera != null)
            {
                float desiredZoom = Mathf.Clamp(distance, minZoomIn, maxZoomOut);
                camera.orthographicSize = Mathf.Lerp(camera.orthographicSize, desiredZoom, zoomSpeed * Time.deltaTime);
            }

            //if one player falls too far behind, respawn them
            if (distance >= 35f)
            {
                if (player1.position.x < player2.position.x)
                {
                    respawnPlayer(player1.gameObject);
                }
                else
                {
                    respawnPlayer(player2.gameObject);
                }
            }
        }
    }

    void respawnPlayer(GameObject player)
    {
        Transform otherPlayer = (player.tag == "Player1") ? player2 : player1;

        // Start just 2 units above the other player, not from too high
        Vector3 rayOrigin = otherPlayer.position + new Vector3(-10f, 0f, 0f);
        float rayDistance = 5f; // Only check a few units downward

        // Cast downward to detect ground
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, LayerMask.GetMask("Ground"));

        if (hit.collider != null)
        {
            // Place the player just above the hit point
            Vector3 safePosition = hit.point + Vector2.up * 1f;
            safePosition.z = 0f;
            player.transform.position = safePosition;
        }
        else
        {
            // If no ground is found, fallback to a default spot
            Vector3 fallback = otherPlayer.position + new Vector3(-10f, 1f, 0f);
            fallback.z = 0f;
            Debug.LogWarning("Ground not found. Using fallback position.");
            player.transform.position = fallback;
        }
    }
}
