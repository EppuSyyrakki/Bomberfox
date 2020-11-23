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
        public GameObject mineEmpty;
        public GameObject mineDisabled;

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
            playerHealth.text = pc.ReturnHealth().ToString();
            bombAmount.text = pc.maxBombs.ToString();
        }

        public void ShowSpecialBombs()
        {
			PlayerController pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

			if (pc.megaBomb == null)
			{
                // TODO HIDE MEGA BOMB

				//bigBombEmpty.SetActive(true);
				//remoteEmpty.SetActive(true);
				//bigBombDisabled.SetActive(false);
				//remoteDisabled.SetActive(false);
			}
			
			if (pc.remoteBomb == null)
			{
                // TODO HIDE REMOTE BOMB

				//bigBombEmpty.SetActive(false);
				//remoteEmpty.SetActive(false);
				//remoteDisabled.SetActive(true);
			}
			
			if (pc.mineBomb == null)
			{
                // TODO HIDE MINE BOMB

				//remoteEmpty.SetActive(false);
				//bigBombEmpty.SetActive(false);
				//bigBombDisabled.SetActive(true);
			}
		}
    }
}
