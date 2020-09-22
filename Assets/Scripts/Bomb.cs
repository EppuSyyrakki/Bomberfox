using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField, Tooltip("In seconds")]
    private float bombTimer = 2f;

    [SerializeField, Tooltip("How far the explosion goes")] [Range(1, 10)]
    private int range = 1;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (bombTimer > 0)
        {
            bombTimer -= Time.deltaTime;
        }
        else
        {
            Explode();
        }
    }

    // Destroys the bomb
    private void Explode()
    {
        Destroy(this.gameObject);
    }
}
