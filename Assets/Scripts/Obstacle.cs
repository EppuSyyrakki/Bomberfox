using System;
using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
	private List<Fader> childFaders = new List<Fader>();
	private List<Rigidbody2D> childRbs = new List<Rigidbody2D>();
	private BoxCollider2D boxCollider2D;

    [SerializeField] 
    private GameObject levelEndKey = null;
    [SerializeField] 
    private float explosionForce = 10f;

    public bool IsKey
    {
        private get; 
        set;
    }

	// Start is called before the first frame update
	void Start()
	{
		childFaders.AddRange(GetComponentsInChildren<Fader>());
	    childRbs.AddRange(GetComponentsInChildren<Rigidbody2D>());
	    boxCollider2D = GetComponent<BoxCollider2D>();
    }
	
    public void BlowUp(Vector3 direction)
    {
	    float waitTime = 1f;
		Destroy(boxCollider2D);

	    foreach (Fader fader in childFaders)
	    {
		    fader.Fade = true;
		    waitTime = fader.fadeOutTime;
	    }

	    foreach (Rigidbody2D rb in childRbs)
	    {
			Vector2 explosionDir = new Vector2(direction.x, direction.y);
		    Vector2 dir = new Vector2(
			    Random.Range(-explosionForce, explosionForce), 
			    Random.Range(-explosionForce, explosionForce));
			rb.AddForce(dir + explosionDir * Random.Range(50, 100f));
		}

        if (IsKey)
        {
            Instantiate(levelEndKey, transform.position, Quaternion.identity);
        }

        Invoke(nameof(Remove), waitTime * 1.1f);
    }

    private void Remove() => Destroy(gameObject);

}
