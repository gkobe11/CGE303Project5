using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class LevelMenu : MonoBehaviour
{
    public Button[] buttons;

    private void Awake()
    {
        int unlockedLevel = PlayerPrefs.GetInt("UnlockedLevel", 0);
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].interactable = false;
        }
        for (int i = 0; i <= unlockedLevel; i++)
        {
            buttons[i].interactable = true;
        }
    }

    public void OpenLevel(int levelId)
    {
        string levelName = "Level" + (levelId);
        SceneManager.LoadScene(levelName);
    }

    /*
    public void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }

    public void SceneOne()
    {
        SceneManager.LoadScene("Level1");
    }

    public void SceneTwo()
    {
        SceneManager.LoadScene("Level2");
    }

    public void SceneThree()
    {
        SceneManager.LoadScene("Level3");
    }
    */
}
