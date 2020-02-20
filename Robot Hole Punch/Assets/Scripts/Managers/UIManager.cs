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

    #region In Game Canvas Settings

    [Header("In Game Canvas")]

    [SerializeField]
    private GameObject inGame;
    [SerializeField]
    private Slider healthBar;
    [SerializeField]
    private Image laserChargeImage;
    
    public Image LaserCharge { get { return laserChargeImage; } }
    public Slider PlayerHealthBar { get { return healthBar; } }

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
        ActivateInGameEnvironment();
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
        inGame.SetActive(true);
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

    private Coroutine smoothHealthCoroutine = null;

    public void ChangeHealth(float newHealth)
    {
        if (smoothHealthCoroutine != null)
            StopCoroutine(smoothHealthCoroutine);
        smoothHealthCoroutine = StartCoroutine(SmoothHealth(newHealth));
    }


    public void SetLaserChargeIntensity(float power)
    {
        power = Mathf.Clamp(power, 0f, 1f);
        laserChargeImage.color = Color.Lerp(Color.green, Color.red, power);
        Debug.Log(laserChargeImage.color);
    }

    private IEnumerator SmoothHealth(float newHealth)
    {
        while (true)
        {
            healthBar.value = Mathf.Lerp(healthBar.value, newHealth, 4f * Time.deltaTime);
            if (Mathf.Abs(healthBar.value - newHealth) < 0.1f)
            {
                healthBar.value = newHealth;
                break;
            }
            yield return null;
        }
    }


}
