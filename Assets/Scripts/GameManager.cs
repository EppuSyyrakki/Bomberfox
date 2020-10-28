using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Bomberfox
{
    public class GameManager : MonoBehaviour
    {
        // NOTE TO SELF: If you need to call manager from somewhere, use GameManager.Instance.something
        public static GameManager instance = null;
        private string scene;

        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    // Add instantiation
                }

                return instance;
            }
        }

        private bool keyCreated;

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

            Scene currentScene = SceneManager.GetActiveScene();
            scene = currentScene.name;
        }

        void Update()
        {
	        if (!keyCreated && scene == "TestingGrounds")
	        {
		        CreateKeyObstacle();
		        keyCreated = true;
            }

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

        public static void ChangeLevel(int levelNumber)
        {
            SceneManager.LoadScene(levelNumber);
        }

        public void CreateKeyObstacle()
        {
            GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
            GameObject keyObstacle = allObstacles[Random.Range(0, allObstacles.Length)];
            keyObstacle.GetComponent<Obstacle>().IsKey = true;
            Debug.Log("Key was hidden in " + keyObstacle.transform.position);

            keyObstacle.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }
}
