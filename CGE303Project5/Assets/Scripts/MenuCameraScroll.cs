using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuCameraScroll : MonoBehaviour
{
    [System.Serializable]
    public class LevelScroll
    {
        public GameObject levelBackground; // Background to activate
        public Transform startPoint;       // Camera start position
        public Transform endPoint;         // Camera end position
    }

    public LevelScroll[] levels;           // Set in Inspector
    public float scrollDuration = 10f;     // Time to scroll across each level
    public float waitBeforeNextLevel = 2f; // Pause between levels

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(ScrollThroughLevels());
    }

    private IEnumerator ScrollThroughLevels()
    {
        foreach (var level in levels)
        {
            // Activate this background, deactivate others
            foreach (var l in levels)
            {
                l.levelBackground.SetActive(false);
            }
            level.levelBackground.SetActive(true);

            // Scroll camera from startPoint to endPoint
            float timer = 0f;
            while (timer < scrollDuration)
            {
                timer += Time.deltaTime;
                float t = timer / scrollDuration;

                Vector3 newPosition = Vector3.Lerp(level.startPoint.position, level.endPoint.position, t);
                newPosition.z = mainCamera.transform.position.z; // Keep original Z
                mainCamera.transform.position = newPosition;

                yield return null;
            }

            yield return new WaitForSeconds(waitBeforeNextLevel);
        }

        // Loop or stop depending on your preference
        StartCoroutine(ScrollThroughLevels()); // Repeat
        // yield break; // Use this instead to scroll once and stop
    }
}
