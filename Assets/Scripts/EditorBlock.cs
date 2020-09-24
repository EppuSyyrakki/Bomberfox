using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, SelectionBase]
public class EditorBlock : MonoBehaviour
{
    [SerializeField] private int gridSize = 1;

    // Update is called once per frame
    private void Update()
    {
        transform.position = new Vector3
        (Mathf.RoundToInt(transform.position.x / gridSize) * gridSize,
            Mathf.RoundToInt(transform.position.y / gridSize) * gridSize,
            0f);
    }
}