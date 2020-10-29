using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Bomberfox;
using UnityEngine;

namespace MyNamespace
{
	public class DeathMenuUIController : MonoBehaviour
	{
		void Start()
        {
            gameObject.SetActive(false);
        }

        void Update()
        {

        }

        public void ToggleEndMenu()
        {
            gameObject.SetActive(true);
        }

        public void Restart()
        {
            GameManager.Instance.ChangeLevel(1);
        }

        public void BackToMenu()
        {
            GameManager.Instance.ChangeLevel(0);
        }
    }
}
