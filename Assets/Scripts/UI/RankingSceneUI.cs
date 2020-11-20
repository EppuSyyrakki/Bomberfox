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
            float killPercentage;

            if (GameManager.Instance.ExplodedBombs == 0)
            {
                killPercentage = 0;
            }
            else
            {
                killPercentage = (float)GameManager.Instance.KilledEnemies / GameManager.Instance.ExplodedBombs * 100;
                killPercentage = Mathf.Round(killPercentage * 100) / 100;
            }

            levelsCleared.text = (GameManager.Instance.FinishedLevels).ToString();
            enemiesBombed.text = (GameManager.Instance.KilledEnemies).ToString();
            playerLevel.text = (GameManager.Instance.PlayerLevel).ToString();
            xpLeft.text = "XP until next level: " +
                          (GameManager.Instance.XpForNextLevel - GameManager.Instance.LevelProgression).ToString();

            statistics.text = "Dropped bombs: " + (GameManager.Instance.ExplodedBombs).ToString() +
                              "\nKill percentage: " + killPercentage.ToString() + "%" +
                                "\nDestroyed obstacles: " + (GameManager.Instance.DestroyedBlocks).ToString() +
                              "\nCollected power ups: " + (GameManager.Instance.CollectedPU).ToString() +
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
