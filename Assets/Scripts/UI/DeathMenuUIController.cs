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
                FindObjectOfType<AudioManager>().StopPlayingGameMusic();
            }

            GameManager.Instance.ResetLevelCounter();
            FindObjectOfType<AudioManager>().CheckGameMusic();

            Debug.Log("Play again pressed");
            GameManager.Instance.ChangeLevel(1);
        }

        public void BackToMenu()
        {
            FindObjectOfType<AudioManager>().StopPlayingGameMusic();
            GameManager.Instance.ChangeLevel(0);
        }
    }
}
