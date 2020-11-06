using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bomberfox
{
    public class GameManager : MonoBehaviour
    {
        // NOTE TO SELF: If you need to call manager from somewhere, use GameManager.Instance.something
        public static GameManager instance = null;

        public GameObject deathMenu;

        public int CurrentLevel { get; set; }

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
	                Debug.Log("Game Manager Instance not found! Add it to the scene from the Prefabs folder");
                }

                return instance;
            }
        }

        void Awake()
        {
            if (instance == null)
            {
                instance = this;
            }
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            DontDestroyOnLoad(gameObject);

            CurrentLevel = 1;
        }

        void Update()
        {
	        if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
            }

            if (Input.GetKeyDown(KeyCode.P))
            {
                Time.timeScale = 0f;
            }

            if (Input.GetKeyUp(KeyCode.P))
            {
                Time.timeScale = 1f;
            }
        }

        public void ChangeLevel(int levelNumber)
        {
            SceneManager.LoadScene(levelNumber);
        }

        public void ReloadScene()
        {
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
        }

        public void ResetLevelCounter()
        {
            Debug.Log("Level counter reset");
            CurrentLevel = 1;
        }
    }
}
