using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    public GameController game_controller;

    private CapsuleCollider2D check;
    private ContactFilter2D filter;
    private Collider2D[] results = new Collider2D[50];
    private List<GameObject> return_objects;

    void Start()
    {
        check = GetComponent<CapsuleCollider2D>();
        filter = new ContactFilter2D();
        filter.NoFilter();
        filter.useTriggers = true;
    }

    public List<GameObject> interaction()
    {
        results = new Collider2D[50];
        check.OverlapCollider(filter, results);
        return_objects = new List<GameObject>();

        for(int i = 0; i < results.Length; i++)
        {
            if (results[i] != null)
            {
                if (results[i].gameObject.tag == "interactable")
                {
                    if (results[i].gameObject.GetComponent<Interactable>() != null)
                    {
                        return_objects.Add(results[i].gameObject);
                    }
                }
            }
        }
        return return_objects;
    }
}
