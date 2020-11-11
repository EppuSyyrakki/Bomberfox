using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class MenuUIController : MonoBehaviour
    {
        void Start()
        {
            AudioManager.instance.PlaySound("Spooky");
            AudioManager.instance.PlaySound("Thunder2");
        }

        public void StartGame()
        {
            Debug.Log("Start pressed");
            AudioManager.instance.CheckGameMusic();
            GameManager.Instance.GoToStory();
        }

        public void QuitGame()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
        }
    }
}


