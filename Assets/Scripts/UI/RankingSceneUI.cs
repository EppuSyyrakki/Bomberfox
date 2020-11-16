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

        public void PrintStats()
        {
            levelsCleared.text = (GameManager.Instance.CurrentLevel - 1).ToString();
            enemiesBombed.text = (GameManager.Instance.KilledEnemies).ToString();

            statistics.text = "Dropped bombs: " + (GameManager.Instance.ExplodedBombs).ToString() + 
                                "\nDestroyed obstacles: " + (GameManager.Instance.DestroyedBlocks).ToString() +
                                "\nKill percentage: xx%";
        }
    }
}
