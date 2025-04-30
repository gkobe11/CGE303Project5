using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class OpenScene : MonoBehaviour
{
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

    public void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
}
