using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class LevelEndKey : MonoBehaviour
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 9, true);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Key"))
        {
            Physics2D.IgnoreLayerCollision(8, 9, true);
            AudioManager.instance.VictoryMusic();
            StartCoroutine(FindObjectOfType<FadeOutUI>().FadeBlackOutSquare());
            GameManager.Instance.CurrentLevel++;
            AudioManager.instance.CheckGameMusic();
            GameManager.Instance.Invoke("ReloadScene", 1.0f);
        }
    }
}
