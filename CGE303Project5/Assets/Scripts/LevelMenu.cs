using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class LevelMenu : MonoBehaviour
{
    [Header("Level Buttons in Order")]
    public Button[] levelButtons; // Assign in Inspector: Level0, Level1, Level2, ...

    private void Start()
    {
        // Check if the tutorial is marked as completed in PlayerPrefs
        bool tutorialComplete = PlayerPrefs.GetInt("TutorialComplete", 0) == 1;

        // Disable all buttons by default
        foreach (Button btn in levelButtons)
        {
            btn.interactable = false;
        }

        if (tutorialComplete)
        {
            // Unlock all levels if tutorial is completed
            foreach (Button btn in levelButtons)
            {
                btn.interactable = true;
            }
        }
        else
        {
            // Only unlock tutorial level (Level0)
            if (levelButtons.Length > 0)
                levelButtons[0].interactable = true;
        }
    }

    // Call this function from each level button and pass its index (0, 1, 2, ...)
    public void OpenLevel(int levelIndex)
    {
        string levelName = "Level" + levelIndex;
        SceneManager.LoadScene(levelName);
    }
}
