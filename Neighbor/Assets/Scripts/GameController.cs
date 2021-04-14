using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour
{
    public AudioSource introSource;

    public Inventory inv_control;
    public FadeController fade_control;
    public TextController text_control;
    public StoryController story_control;
    public GameObject player;
    public Interact interactor;
    public playerController player_control;

    public bool moving;
    public bool finished_moving;
    public Vector2 move_to;
    public List<GameObject> interactables = new List<GameObject>();
    public List<GameObject> options = new List<GameObject>();
    public bool choosing;
    public int choose_count;

    public FadeController intro_fade_txt;
    public FadeController intro_fade_sprt;

    public TextMeshPro introtxt;
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

    public GameObject start_screen;
    public FadeController dark_bg;
    public FadeController initial_fade;
    public FadeController title_name;
    public FadeController start_text;

    public float title_slide;
    public bool title_change;
    public bool title = true;
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
    private bool check_inv = false;
    private bool change_replace;

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
        back_block.gameObject.transform.parent.gameObject.SetActive(true);
        start_screen.SetActive(true);
        initial_fade.opaque = true;
        initial_fade.hold_transparency = true;
    }
    
    public void movePlayerTo(GameObject destination)
    {
        moving = true;
        move_to = destination.transform.position;
        fade_control.opaque = true;
        player_control.frozen = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            introducing = false;
            back_block.gameObject.transform.parent.gameObject.SetActive(false);
            start_screen.SetActive(false);
            player_control.frozen = false;
        }

        if (introducing)
        {
            if (title)
            {
                initial_fade.opaque = false;

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    start_text.pingpong = false;
                    start_text.opaque = false;
                    start_text.hold_transparency = true;
                    dark_bg.opaque = false;
                    title_name.opaque = false;

                    title_slide = introsprt.gameObject.transform.localPosition.x;
                    title_change = true;

                    introSource.Play();
                }

                if (title_change && dark_bg.hold_transparency == false)
                {
                    if (title_slide > 0)
                    {
                        title_slide -= 0.05f + introsprt.gameObject.transform.localPosition.x/30f;
                        introsprt.gameObject.transform.localPosition = new Vector2(title_slide, introsprt.gameObject.transform.localPosition.y);
                    }
                    else
                    {
                        title_slide = 0.0f;
                        introsprt.gameObject.transform.localPosition = new Vector2(title_slide, introsprt.gameObject.transform.localPosition.y);
                        title = false;
                    }
                }
            }
            else
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
                    if (intro_count != 0)
                    {
                        intro_fade_sprt.opaque = false;
                    }
                    else
                    {
                        change_replace = true;
                    }
                }

                if (change_sprite && (!intro_fade_sprt.hold_transparency || change_replace))
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
                    intro_time = 60f;
                    change_slide = false;
                    ready_change = true;
                }
                if (intro_count == 3 && !back_block.hold_transparency)
                {
                    introducing = false;
                    player_control.frozen = false;
                    back_block.gameObject.SetActive(false);
                    intro_fade_sprt.gameObject.SetActive(false);
                    intro_fade_txt.gameObject.SetActive(false);
                    FadeOut();
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                check_inv = !check_inv;

                inv_control.GetComponent<Inventory>().toggleInv();

                if (!check_inv)
                {
                    player_control.frozen = false;
                }
            }

            if (check_inv)
            {
                player_control.frozen = true;

                if (Input.GetKeyDown(KeyCode.W))
                {
                    inv_control.inv_move(-1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.S))
                {
                    inv_control.inv_move(1, 0);
                }
                else if (Input.GetKeyDown(KeyCode.A))
                {
                    inv_control.inv_move(0, -1);
                }
                else if (Input.GetKeyDown(KeyCode.D))
                {
                    inv_control.inv_move(0, 1);
                }
                else if (Input.GetKeyDown(KeyCode.E))
                {
                    inv_control.removeItem();
                }
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
            else if (!check_inv)
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
        target.interaction(this, null, story_control);
    }

    void nextIntro()
    {
        if (intro_count < 3)
        {
            introsprt.sprite = introsprts[intro_count];
            introtxt.text = introtxts[intro_count].text;
            introtxt.gameObject.transform.localPosition = introtxts[intro_count].gameObject.transform.localPosition;

            intro_count++;

            change_sprite_back = true;
            intro_fade_sprt.opaque = true;
            change_replace = false;
        }
        else
        {
            back_block.opaque = false;
        }
    }

    public IEnumerator FadeOut()
    {
        float fadeValue = 0;

        while (fadeValue < 1)
        {
            introSource.volume = Mathf.Lerp(1, 0, fadeValue);
            fadeValue += 0.01f;

            yield return null;
        }

        yield break;
    }
}
