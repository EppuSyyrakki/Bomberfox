﻿using System.Collections;
using System.Collections.Generic;
using Bomberfox.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class MenuUIController : MonoBehaviour
    {
        void Start()
        {
            AudioManager.instance.PlayMusic("MainMenu");
            AudioManager.instance.PlayMusic("ThunderMenu");
        }

        public void Update()
        {

        }

        public void StartGame()
        {
            GameManager.Instance.Player = new PlayerData();
            AudioManager.instance.CheckGameMusic();
            AudioManager.instance.StopMusic("ThunderMenu");
            AudioManager.instance.StopMusic("MainMenu");
            GameManager.Instance.GoToStory();
        }

        public void QuitGame()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
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
