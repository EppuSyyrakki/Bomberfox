using System;
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
        public GameObject remoteEmpty;
        public GameObject mineEmpty;

        private PlayerController pc;
        
        private void Start()
        {
	        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        void Update()
        {
            ShowStats();
            ShowSpecialBombs();
        }

        public void ShowStats()
        {
            playerHealth.text = pc.ReturnHealth().ToString();
            bombAmount.text = pc.maxBombs.ToString();
        }

        public void ShowSpecialBombs()
        {
	        bigBombEmpty.SetActive(pc.megaBomb == null);
	        remoteEmpty.SetActive(pc.remoteBomb == null);
			mineEmpty.SetActive(pc.mineBomb == null);
		}
    }
}
