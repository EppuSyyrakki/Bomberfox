using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] 
    private float speed = 10f;
    [SerializeField, Tooltip("In seconds")]
    private float bombTimer = 2f;
    [SerializeField] 
    private GameObject bombPrefab;

    // Start is called before the first frame update
    void Start()
    {
        // Testing line - obsolete text. Commit this dammit!
    }

    // Update is called once per frame
    void Update()
    {
        ProcessMovement();
        ProcessFire();
    }

    private void ProcessMovement()
    {
        float xOffset = Input.GetAxis("Horizontal") * speed * Time.deltaTime;
        float yOffset = Input.GetAxis("Vertical") * speed * Time.deltaTime;

        // Restrict "player" from moving out of screen, numbers from pixels per unit of graphic
        float newX = Mathf.Clamp(transform.position.x + xOffset, 0.5f, 15.5f);
        float newY = Mathf.Clamp(transform.position.y + yOffset, 0.5f, 8.5f);
        
        // Move "player"
        transform.position = new Vector2(newX, newY);
    }

    private void ProcessFire()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject bomb = Instantiate(bombPrefab, transform.position, Quaternion.identity);
            Destroy(bomb, bombTimer);
        }
    }
}
