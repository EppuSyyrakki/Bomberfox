using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bomberfox.UI
{
    public class LevelNumberUI : MonoBehaviour
    {
        [SerializeField] public TextMeshProUGUI levelNumberText;
        [SerializeField] public TextMeshProUGUI levelNumberNumber;

        public void Start()
        {
            levelNumberText.text = "Level";
            levelNumberNumber.text = levelNumberNumber.text = "~ " + (GameManager.Instance.CurrentLevel).ToString() + " ~";
        }

        void Update()
        {

        }
    }

}
