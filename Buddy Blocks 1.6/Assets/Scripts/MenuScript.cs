using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{

    public Canvas quitMenu;
    public Button startText;
    public Button exitText;
    public Button continueText;

    public GameObject MainController;

    // Universal Audio
    public AudioSource UnivSource;
    private AudioClip ButtonSound;

    bool activeGame;

    // Use this for initialization
    void Start()
    {
        quitMenu = quitMenu.GetComponent<Canvas>();
        startText = startText.GetComponent<Button>();
        exitText = exitText.GetComponent<Button>();
        continueText = continueText.GetComponent<Button>();
        Time.timeScale = 0;
        activeGame = false;
        ButtonSound = MainController.GetComponent<OtherGameControls>().Sounds[2];
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (Time.timeScale == 0)
            {
                ContinueLevel();
            }
            else
            {
                PauseGame();
            }
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            CreateBoxes BoxMaker = MainController.GetComponent<CreateBoxes>();
            if (BoxMaker.bombpocalypse_on == false)
            {
                BoxMaker.bombpocalypse_on = true;

            }
            else
            {
                BoxMaker.bombpocalypse_on = false;

            }
        }
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        GetComponent<Canvas>().enabled = true;
        MainController.GetComponent<CreateBoxes>().paused = true;
    }

    public void ExitPress()
    {
        UnivSource.PlayOneShot(ButtonSound);

        quitMenu.enabled = true;
        startText.enabled = false;
        exitText.enabled = false;
        continueText.enabled = false;
    }

    public void NoPress()
    {
        quitMenu.enabled = false;
        startText.enabled = true;
        exitText.enabled = true;
        continueText.enabled = true;
    }

    public void StartLevel()
    {
        UnivSource.PlayOneShot(ButtonSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        /*
        Time.timeScale = 1;
        foreach (Transform t in gameController.GetComponent<CreateBoxes>().parent.GetComponentInChildren<Transform>())
        {
            Destroy(t.gameObject);
        }
        gameController.GetComponent<CreateBoxes>().StartGame();
        GetComponent<Canvas>().enabled = false;*/
    }

    public void ContinueLevel()
    {
        Time.timeScale = 1;
        if (activeGame == false)
        {
            MainController.GetComponent<CreateBoxes>().StartGame();
            activeGame = true;
            continueText.GetComponent<Text>().text = "Continue";
            startText.GetComponent<Text>().enabled = true;
        }
        else
        {
            MainController.GetComponent<CreateBoxes>().paused = false;
        }
        GetComponent<Canvas>().enabled = false;

        UnivSource.PlayOneShot(ButtonSound);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
