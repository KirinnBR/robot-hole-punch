using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
	#region Data Types
	
	[System.Serializable]
	public enum GameState
	{
		MainMenu = 0, InGame = 1, PauseMenu = 2
	}
	public class GameStateEvent : UnityEvent<GameState> { }

	#endregion

	[SerializeField]
	private GameObject[] SystemPrefabs;
	public GameStateEvent onGameStateChanged = new GameStateEvent();
	public UnityEvent onSceneLoadedComplete = new UnityEvent();

	private List<GameObject> instancedSystemPrefabs;
	private List<AsyncOperation> loadOperations;
	private string currentLevel = string.Empty;

	public GameState CurrentGameState { get; private set; }

	private void Start()
	{
		DontDestroyOnLoad(gameObject);

		loadOperations = new List<AsyncOperation>();
		instancedSystemPrefabs = new List<GameObject>();

		InstantiateSystemPrefabs();

		onGameStateChanged.AddListener(UIManager.Instance.UpdateUI);

		ChangeGameState(GameState.MainMenu);
		onGameStateChanged.Invoke(CurrentGameState);
	}

	public void OnLoadOperationComplete(AsyncOperation ao)
	{
		if (loadOperations.Contains(ao))
		{
			loadOperations.Remove(ao);
			//Dispatch messages.
			//Transition between scenes.
		}
		Debug.Log($"{currentLevel} loaded.");
		onSceneLoadedComplete.Invoke();
	}

	public void OnUnloadOperationComplete(AsyncOperation ao)
	{
		Debug.Log("Unloaded");
	}

	public void LoadLevel(string levelName, LoadSceneMode loadSceneMode)
	{
		var ao = SceneManager.LoadSceneAsync(levelName, loadSceneMode);

		if (ao == null)
		{
			Debug.LogError("[GameManager] unable to load level.");
			return;
		}
		ao.completed += OnLoadOperationComplete;
		loadOperations.Add(ao);
		currentLevel = levelName;
	}

	public void UnloadLevel(string levelName)
	{
		var ao = SceneManager.UnloadSceneAsync(levelName);

		if (ao == null)
		{
			Debug.LogError("[GameManager] unable to unload level.");
			return;
		}

		ao.completed += OnUnloadOperationComplete;
	}

	public void ChangeGameState(GameState state)
	{
		CurrentGameState = state;
		switch (CurrentGameState)
		{
			case GameState.InGame:
				InGameState();
				break;
			case GameState.PauseMenu:
				PauseMenuState();
				break;
		}
	}

	public void PauseMenuState()
	{
		Time.timeScale = 0f;
		CurrentGameState = GameState.PauseMenu;
	}

	public void InGameState()
	{
		Time.timeScale = 1f;
		CurrentGameState = GameState.InGame;
	}

	public void QuitGame()
	{
		Application.Quit();
	}

	private void InstantiateSystemPrefabs()
	{
		GameObject prefabInstance;
		foreach (var prefab in SystemPrefabs)
		{
			prefabInstance = Instantiate(prefab);
			instancedSystemPrefabs.Add(prefabInstance);
		}
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		foreach (var prefab in instancedSystemPrefabs)
		{
			Destroy(prefab);
		}
		instancedSystemPrefabs.Clear();
	}
}
