using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class DeathMenuUIController : MonoBehaviour
    {
        public void RestartGame()
        {
            if (GameManager.Instance.CurrentLevel != 1)
            {
                AudioManager.instance.StopGameMusic();
                GameManager.Instance.ResetLevelCounter();
                AudioManager.instance.CheckGameMusic();

                GameManager.Instance.GoToGame();
            } 
            else if (GameManager.Instance.CurrentLevel == 1)
            {
                GameManager.Instance.ResetLevelCounter();
                GameManager.Instance.GoToGame();
            }
        }

        public void BackToMenu()
        {
            AudioManager.instance.StopGameMusic();
            GameManager.Instance.GoToMainMenu();
        }
    }
}
