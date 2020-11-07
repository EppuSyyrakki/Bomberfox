using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class AnyKey : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // press any key to go to game
        if (Input.anyKeyDown) GameManager.Instance.GoToGame();
    }
}
