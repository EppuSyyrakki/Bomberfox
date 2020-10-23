using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class MenuUIController : MonoBehaviour
    {
        // Start is called before the first frame update
        public void StartGame()
        {
            Debug.Log("Start pressed");
            GameManager.ChangeLevel(1);
        }

        // Update is called once per frame
        public void QuitGame()
        {
            Debug.Log("Quit pressed");
            Application.Quit();
        }
    }
}


