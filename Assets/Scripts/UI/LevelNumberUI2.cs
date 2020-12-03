using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class LevelNumberUI2 : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI levelNumber;

        public void Start()
        {
            levelNumber.text = "~ " + (GameManager.Instance.CurrentLevel).ToString() + " ~";
        }

        void Update()
        {

        }
    }

}
