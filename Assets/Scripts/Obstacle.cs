using System;
using System.Collections;
using System.Collections.Generic;
using Bomberfox;
using UnityEngine;
using Random = UnityEngine.Random;

public class Obstacle : MonoBehaviour
{
	private static GameObject enemiesParent;

	private List<Rigidbody2D> childRbs = new List<Rigidbody2D>();
	private BoxCollider2D boxCollider2D;
	private bool lightDestroyed = false;

    [SerializeField] 
    private GameObject levelEndKey = null;
    [SerializeField] 
    private float explosionForce = 10f;
    [SerializeField]
    private GameObject guideLight = null;

	public GameObject shadow;
	public GameObject shadowCaster;

	public bool IsKey
    {
        private get; 
        set;
    }

	// Start is called before the first frame update
	private void Start()
	{
	    childRbs.AddRange(GetComponentsInChildren<Rigidbody2D>());
	    boxCollider2D = GetComponent<BoxCollider2D>();

	    if (enemiesParent == null)
	    {
		    enemiesParent = GameObject.FindWithTag("EnemyParent");
	    }
	}

	private void Update()
	{
		if (IsKey && enemiesParent.transform.childCount == 0 && !lightDestroyed)
		{
			guideLight.SetActive(true);
		}
	}

	public void BlowUp(Vector3 direction)
    {
		Animator a = GetComponent<Animator>();
		Destroy(a);
		shadow.SetActive(false);
		shadowCaster.SetActive(false);
		float waitTime = 1f;
		Destroy(boxCollider2D);
		Destroy(guideLight);
		lightDestroyed = true;

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
