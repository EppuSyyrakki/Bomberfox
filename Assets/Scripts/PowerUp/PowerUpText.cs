using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Bomberfox.PowerUp
{
    public class PowerUpText : MonoBehaviour
    {
	    [SerializeField]
	    private float lifeTime = 1f;

        [SerializeField]
	    private TMP_Text textField;

        // Start is called before the first frame update
        private void Start()
        {
	        Destroy(gameObject, lifeTime);
        }

        public void SetText(string text)
        {
	        textField.text = text;
        }
    }
}
