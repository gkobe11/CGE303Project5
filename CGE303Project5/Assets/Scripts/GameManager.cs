using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public GameObject winPanel;
    public Text winnerText;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void ShowWinPanel(string winner)
    {
        winnerText.text = winner + " Wins!";
        winPanel.SetActive(true);
    }

    public void BackToLevelSelect()
    {
        // Navigate back to the level selection screen
    }
}