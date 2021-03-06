﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool pickup;
    public bool observe;
    public bool interact;
    public bool door;

    public GameObject inventory_item;

    public string interact_name;
    public GameObject inspect_text;

    public void interaction(GameController game_controller)
    {
        //Debug.Log("interacted with");
        //Debug.Log(interact_name);

        if (observe)
        {
            game_controller.text_control.show_text(inspect_text);
        }
        if (door)
        {
            GetComponent<Door>().useDoor(game_controller);
        }
    }
}
