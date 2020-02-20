using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{

	#region Main Menu Canvas Settings

	[Header("Main Menu Canvas")]

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private Button optionsButton;
    [SerializeField]
    private Button quitGameButton;

    #endregion

    #region Pause Menu Canvas Settings

    [Header("Pause Menu Canvas")]

    [SerializeField]
    private GameObject pauseMenu;
    [SerializeField]
    private Button resumeButton;

    #endregion

    #region Options Menu Canvas Settings

    [Header("Options Menu Canvas")]

    [SerializeField]
    private GameObject optionsMenu;
    [SerializeField]
    private Button backButton;

	#endregion

	protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(Instance);

        //Main Menu buttons.
        startButton.onClick.AddListener(StartButtonPressed);
        optionsButton.onClick.AddListener(OptionsButtonPressed);
        quitGameButton.onClick.AddListener(QuitGameButtonPressed);

        //Pause Menu buttons.
        resumeButton.onClick.AddListener(ResumeButtonPressed);

        //Options Menu buttons.
        backButton.onClick.AddListener(BackButtonPressed);

    }



    #region Main Menu Methods

    private void StartButtonPressed()
    {
        GameManager.Instance.LoadLevel("Level", UnityEngine.SceneManagement.LoadSceneMode.Single);
        GameManager.Instance.ChangeGameState(GameManager.GameState.InGame);
    }

    private void OptionsButtonPressed()
    {
        ActivateOptionsEnvironment();
    }

    private void QuitGameButtonPressed()
    {
        Application.Quit(0);
    }

	#endregion

	#region Pause Menu Methods

    private void ResumeButtonPressed()
    {
        GameManager.Instance.ChangeGameState(GameManager.GameState.InGame);
    }

    #endregion

    #region Options Menu Methods

    private void BackButtonPressed()
    {
        ActivateMainMenuEnvironment();
    }

    #endregion

    public void UpdateUI(GameManager.GameState newState)
    {
        switch (newState)
        {
            case GameManager.GameState.MainMenu:
                ActivateMainMenuEnvironment();
                break;
            case GameManager.GameState.InGame:
                ActivateInGameEnvironment();
                break;
            case GameManager.GameState.PauseMenu:
                ActivatePauseMenuEnvironment();
                break;
        }
    }

    private void ActivateMainMenuEnvironment()
    {
        mainMenu.SetActive(true);
        pauseMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    private void ActivateInGameEnvironment()
    {
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void ActivatePauseMenuEnvironment()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        optionsMenu.SetActive(false);
    }

    private void ActivateOptionsEnvironment()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
    }


}
