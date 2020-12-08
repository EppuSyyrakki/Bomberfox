using Bomberfox.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class RankingSceneUI : MonoBehaviour
    {
        public Text levelsCleared;
        public Text enemiesBombed;
        public Text statistics;
        public Text playerLevel;
        public Text xpLeft;

        public GameObject shieldBronze;
        public GameObject shieldSilver;
        public GameObject shieldGold;

        // Start is called before the first frame update
        void Start()
        {
            PrintStats();
            SetShield();
        }

        public void RestartGame()
        {
            GameManager.Instance.RestartForNewGame();
            AudioManager.instance.StopMusic("DeathSong");
            AudioManager.instance.CheckGameMusic();
            GameManager.Instance.GoToGame();
        }

        public void BackToMenu()
        {
            AudioManager.instance.StopMusic("DeathSong");
            GameManager.Instance.RestartForNewGame();
            GameManager.Instance.GoToMainMenu();
        }

        public void PrintStats()
        {
            levelsCleared.text = (GameManager.Instance.FinishedLevels).ToString();
            enemiesBombed.text = (GameManager.Instance.TotalFinishedLevels).ToString();
            playerLevel.text = (GameManager.Instance.PlayerLevel).ToString();
            xpLeft.text = "Score until next rank: " +
                          (GameManager.Instance.XpForNextLevel - GameManager.Instance.LevelProgression).ToString();

            statistics.text = "Dropped bombs: " + (GameManager.Instance.ExplodedBombs).ToString() +
                              "\nKilled enemies: " + (GameManager.Instance.KilledEnemies).ToString() +
                                "\nDestroyed obstacles: " + (GameManager.Instance.DestroyedBlocks).ToString() +
                              "\nCollected power ups: " + (GameManager.Instance.CollectedPU).ToString() +
                              "\n\nDropped bombs in total: " + (GameManager.Instance.TotalExplodedBombs).ToString() +
                              "\nKilled enemies in total: " + (GameManager.Instance.TotalKilledEnemies).ToString() +
                              "\nDestroyed obstacles in total: " + (GameManager.Instance.TotalDestroyedBlocks).ToString() +
                              "\nCollected power ups in total: " + (GameManager.Instance.TotalCollectedPU).ToString() +
                                "\nPlayer deaths in total: " + (GameManager.Instance.TotalDeaths).ToString();
        }

        public void SetShield()
        {
            if (GameManager.Instance.PlayerLevel < 5)
            {
                shieldBronze.SetActive(true);
                shieldSilver.SetActive(false);
                shieldGold.SetActive(false);
            }
            else if (GameManager.Instance.PlayerLevel < 10)
            {
                shieldBronze.SetActive(false);
                shieldSilver.SetActive(true);
                shieldGold.SetActive(false);
            }
            else if (GameManager.Instance.PlayerLevel >= 10)
            {
                shieldBronze.SetActive(false);
                shieldSilver.SetActive(false);
                shieldGold.SetActive(true);
            }
        }
    }
}
