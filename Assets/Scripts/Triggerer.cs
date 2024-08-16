using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triggerer : MonoBehaviour
{
    public Color color;
    private void OnDrawGizmos()
    {
        BoxCollider2D col = GetComponent<BoxCollider2D>();
        if (col)
        {
            Gizmos.color = color;
            Gizmos.DrawCube(transform.position, col.size);
        }
    }

   
}
