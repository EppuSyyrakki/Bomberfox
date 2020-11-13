using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class DeathMenuUIController : MonoBehaviour
    {
        public void Start()
        {
            GameManager.Instance.PrintStats();
        }

        public void RestartGame()
        {
            GameManager.Instance.ResetLevelCounter();
            GameManager.Instance.ResetPlayThroughStats();
            AudioManager.instance.StopMusic("DeathSong");
            AudioManager.instance.CheckGameMusic();
            GameManager.Instance.GoToGame();
        }

        public void BackToMenu()
        {
            AudioManager.instance.StopMusic("DeathSong");
            GameManager.Instance.GoToMainMenu();
        }
    }
}
