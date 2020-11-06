﻿using System.Collections;
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
            }

            GameManager.Instance.ResetLevelCounter();
            AudioManager.instance.CheckGameMusic();

            Debug.Log("Play again pressed");
            GameManager.Instance.ChangeLevel(1);
        }

        public void BackToMenu()
        {
            AudioManager.instance.StopGameMusic();
            GameManager.Instance.ChangeLevel(0);
        }
    }
}
