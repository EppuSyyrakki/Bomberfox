using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class RankingSceneUI : MonoBehaviour
    {
        public Text levelsCleared;
        public Text enemiesBombed;
        public Text statistics;

        // Start is called before the first frame update
        void Start()
        {
            PrintStats();
        }

        public void RestartGame()
        {
            GameManager.Instance.ResetLevelCounter();
            AudioManager.instance.StopMusic("DeathSong");
            AudioManager.instance.CheckGameMusic();
            GameManager.Instance.GoToGame();
        }

        public void BackToMenu()
        {
            AudioManager.instance.StopMusic("DeathSong");
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

            levelsCleared.text = (GameManager.Instance.CurrentLevel - 1).ToString();
            enemiesBombed.text = (GameManager.Instance.KilledEnemies).ToString();

            statistics.text = "Dropped bombs: " + (GameManager.Instance.ExplodedBombs).ToString() +
                              "\nKill percentage: " + killPercentage.ToString() + "%" +
                                "\nDestroyed obstacles: " + (GameManager.Instance.DestroyedBlocks).ToString() +
                              "\nCollected power ups: " + (GameManager.Instance.CollectedPU).ToString() +
                                "\nPlayer deaths in total: " + (GameManager.Instance.TotalDeaths).ToString() +
                                "\nFinished levels in total: " + (GameManager.Instance.FinishedLevels).ToString() +
                                "\n\nPlayer level: " + (GameManager.Instance.PlayerLevel).ToString() +
                                "\nXP until next level: " + (GameManager.Instance.XpForNextLevel - GameManager.Instance.LevelProgression).ToString();
        }
    }
}
