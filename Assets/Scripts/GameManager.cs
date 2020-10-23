using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

namespace Bomberfox
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager GM;

        void Awake()
        {
            if (GM != null)
            {
                GameObject.Destroy(GM);
            }
            else
            {
                GM = this;
            }

            DontDestroyOnLoad(this);
        }

        void Start()
        {
            CreateKeyObstacle();
        }

        void Update()
        {
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
            Debug.Log("Key was hidden!");

            keyObstacle.GetComponentInChildren<SpriteRenderer>().color = Color.red;
        }
    }
}
