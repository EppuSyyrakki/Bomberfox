using System.Collections;
using System.Collections.Generic;
using Bomberfox.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class GUI : MonoBehaviour
    {
        public Text playerHealth;
        public Text bombAmount;

        public GameObject bigBombEmpty;
        public GameObject bigBombDisabled;
        public GameObject remoteEmpty;
        public GameObject remoteDisabled;

        void Start()
        {
        }

        void Update()
        {
            ShowStats();
            ShowSpecialBombs();
        }

        public void ShowStats()
        {
            PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

            playerHealth.text = pc.playerStartingHealth.ToString();

            bombAmount.text = pc.maxBombs.ToString();
        }

        public void ShowSpecialBombs()
        {
            PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            GameObject bomb = pc.specialBomb;

            if (bomb == null)
            {
                bigBombEmpty.SetActive(true);
                remoteEmpty.SetActive(true);
                bigBombDisabled.SetActive(false);
                remoteDisabled.SetActive(false);
            }
            else if (!bomb.GetComponent<Bomb>().HasRemote)
            {
                bigBombEmpty.SetActive(false);
                remoteEmpty.SetActive(false);
                remoteDisabled.SetActive(true);
            }
            else if (bomb.GetComponent<Bomb>().HasRemote)
            {
                remoteEmpty.SetActive(false);
                bigBombEmpty.SetActive(false);
                bigBombDisabled.SetActive(true);
            }
        }
    }
}
