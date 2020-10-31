using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proximity : MonoBehaviour
{
    private CapsuleCollider2D check;
    private ContactFilter2D filter;
    public Collider2D[] results = new Collider2D[20];
    public List<GameObject> in_proximity;
    private bool prox = false;

    void Start()
    {
        in_proximity = new List<GameObject>();
        check = GetComponent<CapsuleCollider2D>();
        filter = new ContactFilter2D();
        filter.NoFilter();
        filter.useTriggers = true;
    }

    private void Update()
    {
        results = new Collider2D[20];
        check.OverlapCollider(filter, results);
        
        for (int i = 0; i < results.Length; i++)
        {
            if (results[i] != null)
            {
                if (results[i].gameObject.tag == "interactable")
                {
                    if (results[i].gameObject.GetComponent<Door>() != null)
                    {
                        if (!in_proximity.Contains(results[i].gameObject))
                        {
                            results[i].gameObject.GetComponent<Door>().showArrow(true);
                            in_proximity.Add(results[i].gameObject);
                        }
                    }
                }
            }
        }

        prox = false;

        if (in_proximity.Count > 0)
        {
            for (int j = 0; j < results.Length; j++)
            {
                if (results[j] != null)
                {
                    if (in_proximity[0] == results[j].gameObject)
                    {
                        prox = true;
                    }
                }
            }

            if (!prox)
            {
                in_proximity[0].gameObject.GetComponent<Door>().showArrow(false);
                in_proximity.Remove(in_proximity[0]);
            }
        }
    }
}
