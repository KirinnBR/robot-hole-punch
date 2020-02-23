using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{
    #region Other references

    public string SceneName;

    #endregion

    #region Main Menu Canvas Settings

    [Header("Main Menu Canvas")]

    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private Button startButton;
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

    private void Start()
    {
        startButton.onClick.AddListener(StartButtonPressed);
    }

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(Instance);

        //Main Menu buttons.
        startButton.onClick.AddListener(StartButtonPressed);
        quitGameButton.onClick.AddListener(QuitGameButtonPressed);

        //Pause Menu buttons.
        resumeButton.onClick.AddListener(ResumeButtonPressed);

        ActivateMainMenuEnvironment();
    }

    #region Main Menu Methods

    private void StartButtonPressed()
    {
        GameManager.Instance.LoadLevel(SceneName, UnityEngine.SceneManagement.LoadSceneMode.Single);
        GameManager.Instance.ChangeGameState(GameManager.GameState.InGame);
        ActivateInGameEnvironment();
        pauseMenu.SetActive(false);
        mainMenu.SetActive(false);
        inGame.SetActive(true);
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
        inGame.SetActive(false);
    }

    private void ActivateInGameEnvironment()
    {
        inGame.SetActive(true);
        mainMenu.SetActive(false);
        pauseMenu.SetActive(false);
    }

    private void ActivatePauseMenuEnvironment()
    {
        pauseMenu.SetActive(true);
        mainMenu.SetActive(false);
        inGame.SetActive(false);
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
