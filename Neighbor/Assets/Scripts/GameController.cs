using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public FadeController intro_fade_txt;
    public FadeController intro_fade_sprt;

    public TextMeshProUGUI introtxt;
    private TextMeshPro[] introtxts;
    public TextMeshPro introtxt1;
    public TextMeshPro introtxt2;
    public TextMeshPro introtxt3;

    public FadeController back_block;
    public SpriteRenderer introsprt;
    private Sprite[] introsprts;
    public Sprite introsprt1;
    public Sprite introsprt2;
    public Sprite introsprt3;

    public bool introducing = true;
    public bool change_slide;
    public bool change_text;
    public bool change_sprite;
    public bool change_text_back;
    public bool change_sprite_back;
    public int intro_count = 0;
    public float intro_time = 0;
    public bool ready_change = true;
    private bool started;

    void Start()
    {
        introtxts = new TextMeshPro[3];
        introtxts[0] = introtxt1;
        introtxts[1] = introtxt2;
        introtxts[2] = introtxt3;
        introsprts = new Sprite[3];
        introsprts[0] = introsprt1;
        introsprts[1] = introsprt2;
        introsprts[2] = introsprt3;
        player_control.frozen = true;
        back_block.gameObject.SetActive(true);
    }
    
    public void movePlayerTo(GameObject destination)
    {
        moving = true;
        move_to = destination.transform.position;
        fade_control.opaque = false;
        player_control.frozen = true;
    }

    void Update()
    {
        if (introducing)
        {
            if (started)
            {
                if (intro_time > 0)
                {
                    intro_time -= 0.1f;
                }
                else if (ready_change)
                {
                    ready_change = false;
                    change_slide = true;
                }
            }

            if (change_slide)
            {
                change_slide = false;
                change_text = true;
                intro_fade_txt.opaque = false;
            }

            if (change_text && !intro_fade_txt.hold_transparency)
            {
                change_text = false;
                change_sprite = true;
                intro_fade_sprt.opaque = false;
            }

            if (change_sprite && !intro_fade_sprt.hold_transparency)
            {
                change_sprite = false;
                if (!ready_change)
                {
                    nextIntro();
                }

            }

            if (change_sprite_back && intro_fade_sprt.hold_transparency)
            {
                change_sprite_back = false;
                change_text_back = true;
                intro_fade_txt.opaque = true;
            }

            if (change_text_back && intro_fade_txt.hold_transparency)
            {
                change_text_back = false;
                intro_time = 20f;
                change_slide = false;
                ready_change = true;
            }

            if (intro_count==3 && !back_block.hold_transparency)
            {
                introducing = false;
                player_control.frozen = false;
                back_block.gameObject.SetActive(false);
            }
        }

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
            if (introducing)
            {
                started = true;
            }
            else
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
        }

        if (text_control.inspecting)
        {
            if (player_control.input.magnitude > 0.5)
            {
                text_control.clear_text();
            }
        }

        if (moving && fade_control.hold_transparency)
        {
            player.transform.position = move_to;
            fade_control.opaque = false;
            finished_moving = true;
        }
        else if (finished_moving && !fade_control.hold_transparency)
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

    void nextIntro()
    {
        if (intro_count < 3)
        {
            introsprt.sprite = introsprts[intro_count];
            introtxt.GetComponent<TextMeshProUGUI>().text = introtxts[intro_count].text;
            introtxt.GetComponent<TextMeshProUGUI>().transform.localPosition = introtxts[intro_count].transform.localPosition;

            intro_count++;

            change_sprite_back = true;
            intro_fade_sprt.opaque = true;
        }
        else
        {
            back_block.opaque = false;
        }
    }
}
