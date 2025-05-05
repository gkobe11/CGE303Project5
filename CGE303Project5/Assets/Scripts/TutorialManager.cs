using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    public enum TutorialStage
    {
        Move,
        PowerUpCollect,
        PowerUp,
        Goal,
        Done
    }

    public TutorialStage currentStage = TutorialStage.Move;

    //UI Objects
    public GameObject tutorialPanel; // Link to the UI Panel
    public Text tutorialText; // Link to a UI Text element
    public GameObject keysPanel; // Link to the UI Panel with keys
    public GameObject checks;
    public GameObject powerUpListUI; // UI with all powerups (only shown in stage 2)

    //Scripts
    private PlayerHealth player1Health;
    private PlayerHealth player2Health;
    private PlayerPowerUp player1PowerUp;
    private PlayerPowerUp player2PowerUp;

    private bool p1MovedRight = false;
    private bool p1MovedLeft = false;
    private bool p1Jumped = false;
    private bool p2MovedRight = false;
    private bool p2MovedLeft = false;
    private bool p2Jumped = false;
    private bool p1UsedPowerUp = false;
    private bool p2UsedPowerUp = false;

    void Start()
    {
        player1Health = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerHealth>();
        player2Health = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerHealth>();
        player1PowerUp = GameObject.FindGameObjectWithTag("Player1").GetComponent<PlayerPowerUp>();
        player2PowerUp = GameObject.FindGameObjectWithTag("Player2").GetComponent<PlayerPowerUp>();
        ShowStageInstructions();
    }

    void Update()
    {
        switch (currentStage)
        {
            case TutorialStage.Move:
                if (!p1MovedRight && Input.GetKeyDown(KeyCode.D)) {
                    p1MovedRight = true;
                    checks.transform.GetChild(2).gameObject.SetActive(true);
                }
                if (!p1MovedLeft && Input.GetKeyDown(KeyCode.A))
                {
                    p1MovedLeft = true;
                    checks.transform.GetChild(1).gameObject.SetActive(true);
                }
                if (!p1Jumped && Input.GetKeyDown(KeyCode.W))
                {
                    p1Jumped = true;
                    checks.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (!p2MovedLeft && Input.GetKeyDown(KeyCode.J))
                {
                    p2MovedLeft = true;
                    checks.transform.GetChild(3).gameObject.SetActive(true);
                }
                if (!p2MovedRight && Input.GetKeyDown(KeyCode.L))
                {
                    p2MovedRight = true;
                    checks.transform.GetChild(4).gameObject.SetActive(true);
                }
                if (!p2Jumped && Input.GetKeyDown(KeyCode.I))
                {
                    p2Jumped = true;
                    checks.transform.GetChild(5).gameObject.SetActive(true);
                }

                if (p1MovedRight && p1MovedLeft && p1Jumped && p2MovedRight && p2MovedLeft && p2Jumped)
                    AdvanceStage();
                break;

            case TutorialStage.PowerUpCollect:
                checks = GameObject.Find("PowerUpCollectChecks");
                if (player1PowerUp.hasPowerUp)
                    checks.transform.GetChild(0).gameObject.SetActive(true);
                if (player2PowerUp.hasPowerUp)
                    checks.transform.GetChild(1).gameObject.SetActive(true);
                if (player2PowerUp.hasPowerUp && player1PowerUp.hasPowerUp)
                {
                    checks.SetActive(false);
                    AdvanceStage();
                }
                break;

            case TutorialStage.PowerUp:
                checks = GameObject.Find("PowerUpChecks");
                if (!p1UsedPowerUp && Input.GetKeyDown(KeyCode.LeftShift))
                {
                    p1UsedPowerUp = true;
                    checks.transform.GetChild(0).gameObject.SetActive(true);
                }
                if (!p2UsedPowerUp && Input.GetKeyDown(KeyCode.RightShift))
                {
                    p2UsedPowerUp = true;
                    checks.transform.GetChild(1).gameObject.SetActive(true);
                }

                if (p1UsedPowerUp && p2UsedPowerUp)
                {
                    checks.SetActive(false);
                    AdvanceStage();
                }
                break;

            case TutorialStage.Goal:
                if (Input.GetKeyDown(KeyCode.Return))
                    AdvanceStage();
                break;

            case TutorialStage.Done:
                // Tutorial finished
                SceneManager.LoadScene("TutorialFinished");
                break;
        }
    }

    void AdvanceStage()
    {
        currentStage++;
        ShowStageInstructions();
    }

    void ShowStageInstructions()
    {
        keysPanel.SetActive(false);
        powerUpListUI.SetActive(false);

        switch (currentStage)
        {
            case TutorialStage.Move:
                keysPanel.SetActive(true);
                tutorialText.text = "\t\t\t\tPlayer 1: Use WASD \t\t\t\t\t\t\t\t Player 2: Use IJKL to move";
                break;

            case TutorialStage.PowerUpCollect:
                tutorialText.text = "The white boxes contain power ups, pass through them to collect one! You can see what power up you have in the bottom corners of the screen.\n\t\t\t\tPlayer 1: Bottom left \t\t\t\t\t\t\t\t Player 2: Bottom right";
                break;

            case TutorialStage.PowerUp:
                tutorialText.text = "Player 1: Press Left Shift to use a power-up \t\tPlayer 2: Press Right Shift to use a power-up \n\n\nHere are the available power-ups:";
                powerUpListUI.SetActive(true);
                break;

            case TutorialStage.Goal:
                tutorialText.text = "Goal: Be the first to reach the end of the level! \nPress Enter to continue.";
                break;

            case TutorialStage.Done:
                tutorialText.text = "";
                tutorialPanel.SetActive(false);
                break;
        }
    }
}
