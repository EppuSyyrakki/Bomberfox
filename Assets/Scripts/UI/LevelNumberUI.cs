using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class LevelNumberUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI levelNumber;

        public void Start()
        {
            levelNumber.text = "Level " + (GameManager.Instance.CurrentLevel).ToString();
        }

        void Update()
        {

        }
    }

}
