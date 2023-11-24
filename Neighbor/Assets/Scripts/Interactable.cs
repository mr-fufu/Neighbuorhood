using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    public bool pickup;
    public bool removeOnPickup;
    public bool observe;
    public bool interact;
    public bool door;
    public bool locked;
    public bool playAudio;
    public bool unknown;
    [System.NonSerialized] public bool newDoor;
    public bool ladder;
    public bool stairs;

    public GameObject inventoryItem;
    [System.NonSerialized] public bool useInventory;

    public List<InventoryItem> interactItem;
    public List<string> interactItemNames;
    public string interactEvent;

    public string interact_name;
    public GameObject inspect_text;
    public GameObject interactText;
    public string nonMatchText;
    public List<AudioClip> audioToPlay;
    public List<AudioClip> altAudio;

    public List<GameObject> additionalInspectText;

    private void Start()
    {
        if (interactItem.Count > 0)
        {
            interactItemNames = new List<string>(interactItem.Count);

            for (int i = 0; i < interactItem.Count; i++)
            {
                interactItemNames.Add(interactItem[i] != null ? interactItem[i].itemName : null);
            }

            useInventory = true;
        }

        if (door && !stairs)
        {
            newDoor = true;
        }
    }

    public void interaction(GameController gameController, string itemToInteract, StoryController story)
    {
        if (observe || (locked && !interact))
        {
            gameController.text_control.ShowTextFromObject(inspect_text);

            if (interact || pickup)
            {
                observe = false;
            }
            else
            {
                gameController.AddDisabledInteract(this);
            }

            if (locked)
            {
                if (playAudio)
                {
                    gameController.PlayClip(altAudio);
                }
                gameController.text_control.ShowText("It's locked");
            }
            else
            {
                gameController.PlayTrill();
            }
        }
        else if (interact)
        {
            //Debug.Log("Interact triggered on " + this.name);

            if (interactItemNames.Count == 0 || interactItemNames.Contains(itemToInteract))
            {
                //Debug.Log("Used " + itemToInteract + " on " + this.name);

                if (playAudio)
                {
                    //Debug.Log("played audio clip : " + audioToPlay[0].name);
                    gameController.PlayClip(audioToPlay);
                }
                if (interactText != null)
                {
                    gameController.text_control.ShowTextFromObject(interactText);
                }
                gameController.story_control.storyEvent(interactEvent, this, itemToInteract);
            }
            else
            {
                if (interactItemNames.Count > 0)
                {
                    gameController.text_control.ShowText(nonMatchText);

                    if (locked && playAudio)
                    {
                        gameController.PlayClip(altAudio);
                    }
                }
            }
        }
        else if (pickup)
        {
            gameController.inv_control.addItem(inventoryItem);

            if (inventoryItem.GetComponent<InventoryItem>().pickupSound != null)
            {
                gameController.PlayClip(inventoryItem.GetComponent<InventoryItem>().pickupSound);
            }

            if (removeOnPickup)
            {
                Destroy(this.gameObject);
            }
        }
        else if (door)
        {
            GetComponent<Door>().useDoor(gameController);

            gameController.ReEnableInteracts();

            if (playAudio)
            {
                gameController.PlayClip(audioToPlay);
            }
        }
    }

    public string GetInteractType()
    {
        if (unknown)
        {
            return "unknown";
        }
        else if (observe)
        {
            if (locked)
            {
                return "door";
            }
            else
            {
                return "observe";
            }
        }
        else if (interact)
        {
            if (interactItemNames.Count == 0)
            {
                return "interact";
            }
            else
            {
                return "inventory";
            }
        }
        else if (pickup)
        {
            return "pickup";
        }
        else if (door)
        {
            return "door";
        }
        else
        {
            return "observe";
        }
    }
}
