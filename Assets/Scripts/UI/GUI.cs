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
        // Health gauge
        public GameObject healthBase;
        public GameObject health1;
        public GameObject health2;
        public GameObject health3;
        public GameObject health4;
        public GameObject health5;

        // Bomb amount gauge
        public GameObject bombBase;
        public GameObject bomb1;
        public GameObject bomb2;
        public GameObject bomb3;
        public GameObject bomb4;
        public GameObject bomb5;

        // Bomb's range gauge
        public GameObject rangeBase;
        public GameObject range3;
        public GameObject range4;
        public GameObject range5;


        public GameObject bigBombEmpty;
        public GameObject remoteEmpty;
        public GameObject mineEmpty;

        private PlayerController pc;
        
        private void Start()
        {
	        pc = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
            healthBase.SetActive(true);
            bombBase.SetActive(true);
            rangeBase.SetActive(true);
        }

        void Update()
        {
            ShowStats();
            ShowSpecialBombs();
        }

        public void ShowStats()
        {
            // Health gauge
            if (pc.healthSystem.GetHealth() == 0)
            {
                health1.SetActive(false);
                health2.SetActive(false);
                health3.SetActive(false);
                health4.SetActive(false);
                health5.SetActive(false);
            }
            else if (pc.healthSystem.GetHealth() == 1)
            {
                health1.SetActive(true);
                health2.SetActive(false);
                health3.SetActive(false);
                health4.SetActive(false);
                health5.SetActive(false);
            }
            else if (pc.healthSystem.GetHealth() == 2)
            {
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(false);
                health4.SetActive(false);
                health5.SetActive(false);
            }
            else if (pc.healthSystem.GetHealth() == 3)
            {
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(true);
                health4.SetActive(false);
                health5.SetActive(false);
            }
            else if (pc.healthSystem.GetHealth() == 4)
            {
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(true);
                health4.SetActive(true);
                health5.SetActive(false);
            }
            else if (pc.healthSystem.GetHealth() == 5)
            {
                health1.SetActive(true);
                health2.SetActive(true);
                health3.SetActive(true);
                health4.SetActive(true);
                health5.SetActive(true);
            }

            // Bomb amount gauge
            if (pc.maxBombs == 0)
            {
                bomb1.SetActive(false);
                bomb2.SetActive(false);
                bomb3.SetActive(false);
                bomb4.SetActive(false);
                bomb5.SetActive(false);
            }
            else if (pc.maxBombs == 1)
            {
                bomb1.SetActive(true);
                bomb2.SetActive(false);
                bomb3.SetActive(false);
                bomb4.SetActive(false);
                bomb5.SetActive(false);
            }
            else if (pc.maxBombs == 2)
            {
                bomb1.SetActive(true);
                bomb2.SetActive(true);
                bomb3.SetActive(false);
                bomb4.SetActive(false);
                bomb5.SetActive(false);
            }
            else if (pc.maxBombs == 3)
            {
                bomb1.SetActive(true);
                bomb2.SetActive(true);
                bomb3.SetActive(true);
                bomb4.SetActive(false);
                bomb5.SetActive(false);
            }
            else if (pc.maxBombs == 4)
            {
                bomb1.SetActive(true);
                bomb2.SetActive(true);
                bomb3.SetActive(true);
                bomb4.SetActive(true);
                bomb5.SetActive(false);
            }
            else if (pc.maxBombs == 5)
            {
                bomb1.SetActive(true);
                bomb2.SetActive(true);
                bomb3.SetActive(true);
                bomb4.SetActive(true);
                bomb5.SetActive(true);
            }

            // Range gauge
            if (pc.bombRange == 2)
            {
                range3.SetActive(false);
                range4.SetActive(false);
                range5.SetActive(false);
            }
            else if (pc.bombRange == 3)
            {
                range3.SetActive(true);
                range4.SetActive(false);
                range5.SetActive(false);
            }
            else if (pc.bombRange == 4)
            {
                range3.SetActive(true);
                range4.SetActive(true);
                range5.SetActive(false);
            }
            else if (pc.bombRange == 5)
            {
                range3.SetActive(true);
                range4.SetActive(true);
                range5.SetActive(true);
            }
        }

        public void ShowSpecialBombs()
        {
	        bigBombEmpty.SetActive(pc.megaBomb == null);
	        remoteEmpty.SetActive(pc.remoteBomb == null);
			mineEmpty.SetActive(pc.mineBomb == null);
		}
    }
}
