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
            AudioManager.instance.VictoryMusic();
            GameManager.Instance.CurrentLevel++;
            AudioManager.instance.CheckGameMusic();
            Debug.Log("Level: " + GameManager.Instance.CurrentLevel);
            GameManager.Instance.ReloadScene();
        }
    }
}
