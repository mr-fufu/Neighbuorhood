using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryIndicator : MonoBehaviour
{
    public FadeController indicator;
    private Interactable gate;

    void Start()
    {
        gate = GetComponent<Interactable>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gate.observe)
        {
            indicator.opaque = true;
        }

        if (indicator.opaque == true)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                indicator.opaque = false;
                Destroy(this);
            }
        }
    }
}
