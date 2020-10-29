using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public FadeController fade_control;
    public TextController text_control;
    public GameObject player;
    public Interact interactor;
    public playerController player_control;

    public bool moving;
    public bool finished_moving;
    public Vector2 move_to;
    private List<GameObject> interactables = new List<GameObject>();
    private List<GameObject> options = new List<GameObject>();
    private bool choosing;
    private int choose_count;

    void Start()
    {
        
    }
    
    public void movePlayerTo(GameObject destination)
    {
        moving = true;
        move_to = destination.transform.position;
        fade_control.black = true;
        player_control.frozen = true;
    }

    void Update()
    {
        if (choosing)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                options[choose_count].GetComponent<choice>().deselect();

                choose_count -= 1;

                if (choose_count < 0)
                {
                    choose_count = interactables.Count - 1;
                }

                options[choose_count].GetComponent<choice>().select();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                options[choose_count].GetComponent<choice>().deselect();

                choose_count += 1;

                if (choose_count >= interactables.Count)
                {
                    choose_count = 0;
                }

                options[choose_count].GetComponent<choice>().select();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!choosing)
            {
                if (!player_control.frozen)
                {
                    if (!text_control.inspecting)
                    {
                        interactables = interactor.GetComponent<Interact>().interaction();

                        if (interactables.Count == 0)
                        {

                        }
                        else if (interactables.Count == 1)
                        {
                            interact_with(interactables[0].GetComponent<Interactable>());
                        }
                        else
                        {
                            player_control.frozen = true;
                            choosing = true;
                            text_control.inspect_menu_object.SetActive(true);
                            text_control.clear_options();

                            for (int n = 0; n < interactables.Count; n++)
                            {
                                options.Add(text_control.create_option(interactables[n].GetComponent<Interactable>().interact_name));
                            }

                            choose_count = 0;
                            options[0].GetComponent<choice>().select();
                        }
                    }
                    else
                    {
                        //text_control.clear_text();
                    }
                }
            }
            else
            {
                interact_with(interactables[choose_count].GetComponent<Interactable>());
                text_control.clear_options();
                options.Clear();
                text_control.inspect_menu_object.SetActive(false);
                player_control.frozen = false;
                choosing = false;
            }
        }

        if (text_control.inspecting)
        {
            if (player_control.input.magnitude > 0.5)
            {
                text_control.clear_text();
            }
        }

        if (moving && fade_control.hold_black)
        {
            player.transform.position = move_to;
            fade_control.black = false;
            finished_moving = true;
        }
        else if (finished_moving && !fade_control.hold_black)
        {
            finished_moving = false;
            moving = false;
            player_control.frozen = false;
        }
    }

    void interact_with(Interactable target)
    {
        target.interaction(this);
    }
}
