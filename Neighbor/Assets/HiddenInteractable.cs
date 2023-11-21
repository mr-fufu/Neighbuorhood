using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HiddenInteractable : MonoBehaviour
{
    public List<GameObject> hiddenObjects;

    private void Start()
    {
        foreach (GameObject toHide in hiddenObjects)
        {
            toHide.SetActive(false);
        }
    }

    public void Show()
    {
        foreach (GameObject toShow in hiddenObjects)
        {
            toShow.SetActive(true);
        }
    }
}
