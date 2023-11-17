using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    public string itemName;
    public string description;
    public List<AudioClip> pickupSound;

    [System.NonSerialized] public Interactable itemInteract;

    void Start()
    {
        itemInteract = GetComponent<Interactable>();

        if (itemInteract == null)
        {
            Debug.Log(this.gameObject + " has no Interactable script!");
        }
    }
}
