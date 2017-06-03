using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ViewSight : MonoBehaviour {
    [HideInInspector]    public List<Collider2D> colliders = new List<Collider2D>();
    public int amplitude = 0;
    public LayerMask enemyLayerMask = 1 << 8;
    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.transform == transform.parent)
            return;
        colliders.Add(c);
    }

    void OnTriggerExit2D(Collider2D c)
    {
        if (c.transform == transform.parent)
            return;
        colliders.Remove(c);
    }

    public List<Collider2D> getVisibles(float angle)
    {
        List<Collider2D> visibles = new List<Collider2D>();

        float colliderAngle;
        RaycastHit2D hit;
        foreach (Collider2D c in colliders)
        {
            colliderAngle = Vector2.Angle(transform.position, c.transform.position);
            if (angle + amplitude >= colliderAngle && angle + amplitude >= colliderAngle)
            {
                hit = Physics2D.Raycast(transform.position, c.transform.position, Mathf.Infinity, enemyLayerMask);
                if(hit)
                    visibles.Add(c);
            }
        }
        return visibles;
    }

    public List<Collider2D> getVisibles(float angle, string tag)
    {
        List<Collider2D> visibles = new List<Collider2D>();
        float colliderAngle;
        RaycastHit2D hit;
        foreach (Collider2D c in colliders)
        {
            if (c.tag != tag)
                continue;
            
          colliderAngle = Vector2.Angle(transform.position, c.transform.position);
            if (angle + amplitude >= colliderAngle && angle + amplitude >= colliderAngle)
            {
               hit = Physics2D.Raycast(transform.position, c.transform.position, Mathf.Infinity, enemyLayerMask);
                if (hit && hit.collider.tag == tag)
                {
                    visibles.Add(c);
                }
            }
        }
        return visibles;
    }
}
