﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Bomberfox.Player;
using Bomberfox.UI;
using Random = UnityEngine.Random;

namespace Bomberfox
{
	public class GameManager : MonoBehaviour
	{
		// Names of scenes as strings. CHANGE HERE IF BUILD SETTINGS CHANGE
		public const int MainMenu = 0;
		public const int Game = 1;
		public const int DeathMenu = 2;
		public const int Story = 3;

		private static GameManager instance = null;

		public GameObject deathMenu;

		public PlayerData Player { get; set; }

		public int CurrentLevel { get; set; }

        public int ExplodedBombs { get; set; }
        public int KilledEnemies { get; set; }
        public int DestroyedBlocks { get; set; }
        public int CollectedPU { get; set; }
        public int FinishedLevels { get; set; }
        public int TotalDeaths { get; set; }

        public int TotalExplodedBombs { get; set; }
        public int TotalKilledEnemies { get; set; }
        public int TotalDestroyedBlocks { get; set; }
        public int TotalCollectedPU { get; set; }
        public int TotalFinishedLevels { get; set; }

		public bool isPaused = false;

        public bool IsFirstKill { get; set; } = true;

        public int LevelProgression { get; set; }
        public int XpForNextLevel = 100;
        public int PlayerLevel { get; set; } = 1;

        // Power ups will be stored here
        [SerializeField] private GameObject[] allPowerUps = null;

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
			Player = new PlayerData();

            FetchPrefs();
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
			CurrentLevel = 1;
		}

		public void GoToGame() => SceneManager.LoadScene(Game);

		public void GoToMainMenu() => SceneManager.LoadScene(MainMenu);

		public void GoToDeathMenu() => SceneManager.LoadScene(DeathMenu);

		public void GoToStory() => SceneManager.LoadScene(Story);

        // Returns a random power up for the dying enemy.
        // Depending on the current level, extracts few power ups from the list to balance the game.
        public GameObject GetPowerUp()
        {
            if (CurrentLevel < 6)
            {
                GameObject pu = allPowerUps[Random.Range(0, allPowerUps.Length)];
                return pu;
			}
			else if (CurrentLevel < 11)
            {
                GameObject pu = allPowerUps[Random.Range(0, (allPowerUps.Length - 3))];
                return pu;
			}
            else
            {
                GameObject pu = allPowerUps[Random.Range(0, (allPowerUps.Length - 6))];
                return pu;
			}


        }

        public GameObject GetFirstExtraBomb()
        {
            GameObject pu = allPowerUps[1];
            return pu;
        }

        // Checks the player's level progression and if new level was reached, acts according to it.
        public void CheckLevelProgression()
        {
            if (LevelProgression >= XpForNextLevel)
            {
                PlayerLevel++;
                LevelProgression = LevelProgression - XpForNextLevel;

                if (PlayerLevel < 11)
                {
                    XpForNextLevel += 100;
                }
            }
        }

		// Resets the most common variables for the next game.
		// Located at PauseMenu and RankingSceneUI script.
        public void RestartForNewGame()
        {
            IsFirstKill = true;
			Player = new PlayerData();
            ResetLevelCounter();
			ResetStatCounters();
        }

        // Updates total statistics according to the latest game and saves them to PlayerPrefs
        public void UpdateTotalStats()
        {
            TotalExplodedBombs += ExplodedBombs;
            TotalKilledEnemies += KilledEnemies;
            TotalDestroyedBlocks += DestroyedBlocks;
            TotalCollectedPU += CollectedPU;
            TotalFinishedLevels += FinishedLevels;
            CheckLevelProgression();

            PlayerPrefs.SetInt("TotalBombs", TotalExplodedBombs);
            PlayerPrefs.SetInt("TotalEnemies", TotalKilledEnemies);
            PlayerPrefs.SetInt("TotalBlocks", TotalDestroyedBlocks);
            PlayerPrefs.SetInt("CollectedPU", TotalCollectedPU);
            PlayerPrefs.SetInt("TotalLevels", TotalFinishedLevels);

            PlayerPrefs.SetInt("PlayerLevel", PlayerLevel);
            PlayerPrefs.SetInt("LevelProgression", LevelProgression);
            PlayerPrefs.SetInt("XP", XpForNextLevel);
            PlayerPrefs.Save();
        }

        // Resets temporary stats
        public void ResetStatCounters()
        {
            ExplodedBombs = 0;
            KilledEnemies = 0;
            DestroyedBlocks = 0;
            CollectedPU = 0;
            FinishedLevels = 0;
        }

        // When the game starts, this method gets all permanent stats and player's level
        // progression from PlayerPrefs
        public void FetchPrefs()
        {
            TotalExplodedBombs = PlayerPrefs.GetInt("TotalBombs");
            TotalKilledEnemies = PlayerPrefs.GetInt("TotalEnemies");
            TotalDestroyedBlocks = PlayerPrefs.GetInt("TotalBlocks");
            TotalCollectedPU = PlayerPrefs.GetInt("CollectedPU");
            TotalFinishedLevels = PlayerPrefs.GetInt("TotalLevels");

            PlayerLevel = PlayerPrefs.GetInt("PlayerLevel");
            LevelProgression = PlayerPrefs.GetInt("LevelProgression");
            XpForNextLevel = PlayerPrefs.GetInt("XP");
        }
    }
}
