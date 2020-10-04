using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
	private List<Fader> childFaders;
	private List<Rigidbody2D> childRbs;
	private BoxCollider2D boxCollider2D;

	// Start is called before the first frame update
    void Start()
    {
	    childFaders = new List<Fader>(GetComponentsInChildren<Fader>());
	    childRbs = new List<Rigidbody2D>(GetComponentsInChildren<Rigidbody2D>());
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
			    Random.Range(-200f, 200f), 
			    Random.Range(-200f, 200f));
			rb.AddForce(dir + explosionDir * Random.Range(100f, 200f));
		}

		Invoke(nameof(Remove), waitTime * 1.1f);
    }

    private void Remove() => Destroy(gameObject);

}
