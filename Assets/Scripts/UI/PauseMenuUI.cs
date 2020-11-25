using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;

        void Update()
        {
            if (GameManager.Instance.isPaused)
            {
                ActivateMenu();
            }
            else
            {
                DeactivateMenu();
            }
        }

        public void ActivateMenu()
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            AudioManager.instance.MuteBomb();
        }

        public void DeactivateMenu()
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);

            if (GameManager.Instance.isSoundOn)
            {
                AudioManager.instance.EnableBomb();
            }
        }

        public void Continue()
        {
            GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
        }

        public void QuitToMenu()
        {
            GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
            GameManager.Instance.TotalDeaths++;
            GameManager.Instance.UpdateTotalStats();
            GameManager.Instance.RestartForNewGame();
            AudioManager.instance.StopGameMusic();
            GameManager.Instance.GoToMainMenu();
        }

        public void ToggleMusic()
        {
            AudioManager.instance.ToggleMusic();
        }

        public void ToggleSound()
        {
            AudioManager.instance.ToggleSound();
        }
    }
}
