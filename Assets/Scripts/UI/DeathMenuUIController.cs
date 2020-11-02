﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class DeathMenuUIController : MonoBehaviour
    {
        public void RestartGame()
        {
            Debug.Log("Play again pressed");
            GameManager.Instance.ChangeLevel(1);
        }

        public void BackToMenu()
        {
            GameManager.Instance.ChangeLevel(0);
        }
    }
}
