using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Bomberfox
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GM;

        private bool keyCreated;

        void Awake()
        {
            if (GM != null)
            {
                Destroy(GM);
            }
            else
            {
                GM = this;
            }

            DontDestroyOnLoad(this);
        }

        void Update()
        {
	        if (!keyCreated)
	        {
		        CreateKeyObstacle();
		        keyCreated = true;
	        }

            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex));
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
