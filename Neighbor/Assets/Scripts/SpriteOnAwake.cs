using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteOnAwake : MonoBehaviour
{
    public bool interactable;
    public bool sprite;

    public bool enable;

    // Start is called before the first frame update
    void Start()
    {
        if (sprite)
        {
            GetComponent<SpriteRenderer>().enabled = enable;
        }
        if (interactable)
        {
            GetComponent<Interactable>().enabled = enable;
        }
    }

    // Update is called once per frame
}
