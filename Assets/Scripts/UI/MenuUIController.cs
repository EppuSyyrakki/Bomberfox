using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class MenuUIController : MonoBehaviour
    {
        public void StartGame()
        {
            Debug.Log("Start pressed");
            GameManager.Instance.ChangeLevel(1);
        }

        public void QuitGame()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
        }
    }
}


