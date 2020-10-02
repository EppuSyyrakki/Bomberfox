using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class Goat : Enemy
{
    [SerializeField]
    private Vector3 startingPoint = new Vector3(6, 2);

    // Start is called before the first frame update
    void Start()
    {
        collisionHandler = GetComponent<CollisionHandler>();
        SetStartingPoint(startingPoint);
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }
}
