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
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
            }

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
        }

        public void DeactivateMenu()
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
        }

        public void Continue()
        {
            GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
        }

        public void QuitToMenu()
        {
            GameManager.Instance.isPaused = !GameManager.Instance.isPaused;
            GameManager.Instance.ResetLevelCounter();
            GameManager.Instance.ResetPlayThroughStats();
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
