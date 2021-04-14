using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool pickup;
    public bool removeOnPickup;
    public bool observe;
    public bool interact;
    public bool door;
    public bool locked;
    
    public bool useInventory;
    public GameObject inventory_item;

    public GameObject interact_item;
    public string interact_event;

    public string interact_name;
    public GameObject inspect_text;
    public GameObject interact_text;

    public void interaction(GameController game_controller, GameObject item_to_interact, StoryController story)
    {
        if (observe)
        {
            game_controller.text_control.show_text(inspect_text);
        }
        if (door)
        {
            if (!locked)
            {
                GetComponent<Door>().useDoor(game_controller);
            }
        }
        if (pickup)
        {
            game_controller.text_control.show_text(inspect_text);
            game_controller.inv_control.addItem(inventory_item);
            if (removeOnPickup)
            {
                //Destroy(this.gameObject);
            }
        }
        if (interact)
        {
            if (item_to_interact == interact_item)
            {
                game_controller.text_control.show_text(interact_text);
                game_controller.story_control.storyEvent(interact_event);
            }
            else
            {
                game_controller.text_control.show_text(inspect_text);
            }
        }
    }
}
