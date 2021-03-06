using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistancePerception : Perception
{
    public override GameObject[] GetGameObjects()
    {
        List<GameObject> result = new List<GameObject>();

        Collider[] colliders = Physics.OverlapSphere(transform.position, distance);
        foreach (Collider collider in colliders) 
        {
            if (collider.gameObject == gameObject) continue;

            if (tagName == "" || collider.CompareTag(tagName))
            {
                Vector3 direction = (collider.transform.position - transform.position).normalized;
                float dot = Vector3.Dot(transform.forward, direction);
                float angle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                if (angle <= this.angle)
                { 
                    result.Add(collider.gameObject);
                }
            }
        }

        return result.ToArray();
    }
}
