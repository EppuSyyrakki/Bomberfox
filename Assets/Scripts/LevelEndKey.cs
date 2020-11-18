using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using Bomberfox.Player;
using UnityEngine;

public class LevelEndKey : MonoBehaviour
{
    void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 9, true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player") && gameObject.CompareTag("Key"))
        {
	        PlayerController pc = other.gameObject.GetComponent<PlayerController>();
	        GameManager.Instance.Player = pc.GetPlayerData();
            GameManager.Instance.LevelProgression += 20;
	        Physics2D.IgnoreLayerCollision(8, 9, true);
            AudioManager.instance.VictoryMusic();
            StartCoroutine(FindObjectOfType<FadeOutUI>().FadeBlackOutSquare());
            GameManager.Instance.CurrentLevel++;
            GameManager.Instance.FinishedLevels++;
            AudioManager.instance.CheckGameMusic();
            GameManager.Instance.Invoke("ReloadScene", 1.0f);
        }
    }
}
