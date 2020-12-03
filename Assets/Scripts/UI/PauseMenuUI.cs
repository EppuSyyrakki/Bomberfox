using System;
using System.Collections;
using System.Collections.Generic;
using Bomberfox.Player;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Bomberfox.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject pauseMenuUI;

        private void Start()
        {
	        DeactivateMenu();
        }

        public void ActivateMenu()
        {
            Time.timeScale = 0;
            pauseMenuUI.SetActive(true);
            Button btn = GameObject.Find("Continue Button").GetComponent<Button>();
            EventSystem es = GameObject.Find("EventSystem").GetComponent<EventSystem>();
            es.SetSelectedGameObject(null);
            es.SetSelectedGameObject(btn.gameObject);
            AudioManager.instance.PauseAllSounds(true);
        }

        public void DeactivateMenu()
        {
            Time.timeScale = 1;
            pauseMenuUI.SetActive(false);
            GameManager.Instance.isPaused = false;
            AudioManager.instance.PauseAllSounds(false);

            // delay the movementEnabled on PlayerController for a frame to prevent leaving a bomb
            // when using controller button to exit menu
            Invoke(nameof(Continue), Time.deltaTime);
        }
        
        private void Continue()
        {
	        PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            pc.EnableMovement();
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
