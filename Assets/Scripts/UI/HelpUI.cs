using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Bomberfox.UI
{
    public class HelpUI : MonoBehaviour
    {
        [SerializeField] private GameObject help;

        void Start()
        {

        }

        public void ActivateMenu()
        {
            help.SetActive(true);
        }

        public void DeactivateMenu()
        {
            help.SetActive(false);
        }
    }
}
