using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Bomberfox
{
	public class GameManager : MonoBehaviour
	{
		// Names of scenes as strings. CHANGE HERE IF BUILD SETTINGS CHANGE
		public const int MainMenu = 0;
		public const int Game = 1;
		public const int DeathMenu = 2;
		public const int Story = 3;

		// NOTE TO SELF: If you need to call manager from somewhere, use GameManager.Instance.something
		public static GameManager instance = null;

		public GameObject deathMenu;

		public int CurrentLevel { get; set; }

		private bool isPaused = false;

		public static GameManager Instance
		{
			get
			{
				if (instance == null)
				{
					Debug.Log("Game Manager Instance not found! Add it to the scene from the Prefabs folder");
				}

				return instance;
			}
		}

		void Awake()
		{
			if (instance == null)
			{
				instance = this;
			}
			else if (instance != this)
			{
				Destroy(gameObject);
				return;
			}

			DontDestroyOnLoad(gameObject);

			CurrentLevel = 1;
		}

		void Update()
		{
			if (Input.GetKey(KeyCode.R))
			{
				ReloadScene();
			}

			if (Input.GetKeyDown(KeyCode.P) && !isPaused)
			{
				Time.timeScale = 0f;
				isPaused = true;
			}

			if (Input.GetKeyUp(KeyCode.P) && isPaused)
			{
				Time.timeScale = 1f;
				isPaused = false;
			}

			if (Input.GetKeyDown(KeyCode.Escape))
			{
				GoToMainMenu();
			}
		}

		public void ChangeLevel(int levelNumber)
		{
			SceneManager.LoadScene(levelNumber);
		}

		public void ReloadScene()
		{
			SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
		}

		public void ResetLevelCounter()
		{
			Debug.Log("Level counter reset");
			CurrentLevel = 1;
		}

		public void GoToGame() => SceneManager.LoadScene(Game);

		public void GoToMainMenu() => SceneManager.LoadScene(MainMenu);

		public void GoToDeathMenu() => SceneManager.LoadScene(DeathMenu);

		public void GoToStory() => SceneManager.LoadScene(Story);
	}

}
