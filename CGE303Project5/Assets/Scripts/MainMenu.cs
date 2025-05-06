using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour
{
    public void MenuScene()
    {
        SceneManager.LoadScene("MenuScene");
    }

    public void SelectScene()
    {
        SceneManager.LoadScene("LevelSelectScene");
    }
}
