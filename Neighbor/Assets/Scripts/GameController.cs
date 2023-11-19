using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable] public enum SurfaceType { EXTERIOR, HARDWOOD, FLOORBOARD, TILE, CEMENT, CARPET }

public class GameController : MonoBehaviour
{
    public AudioSource introSource;
    public List<AudioClip> ambientOutside;
    public List<AudioClip> drone;
    public List<AudioClip> ambientInside;
    public List<AudioClip> staticClip;
    public List<AudioClip> kitchen;
    public List<AudioClip> metal;

    public AudioController audioControl;
    public Inventory inv_control;
    public FadeController fade_control;
    public TextController text_control;
    public StoryController story_control;
    public GameObject player;
    public Interact interactor;
    public playerController player_control;
    public SlideController slideControl;
    public GameObject escapeMenu;

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

    private float titleSlide;
    private bool titleChange;
    private bool title = true;
    private bool introducing = true;
    private bool changeSlide;
    private bool changeText;
    private bool changeSprite;
    private bool changeTextBack;
    private bool changeSpriteBack;
    private int introCount = 0;
    private float introTime = 0;
    private bool readyChange = true;
    private bool started;
    private bool checkInv = false;
    private bool changeReplace;
    public SurfaceType roomType;

    public List<Sprite> inspectSprite;
    public List<AudioClip> inspectTrill;
    private int inspectTrillCount = 0;
    private InventoryItem selectedItem;
    private List<Interactable> disabledInteracts = new List<Interactable>();
    private float inspectDelay = 0;
    private bool useItem = false;

    void Start()
    {
        roomType = SurfaceType.EXTERIOR;
        audioControl.ChangeClip(ambientOutside);

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
        initial_fade.hold_opaque = true;
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
        if (inspectDelay > 0)
        {
            inspectDelay -= Time.deltaTime;
        }
        else if (inspectDelay < 0)
        {
            inspectDelay = 0;
        }

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

                if (Input.GetKeyDown(KeyCode.Space) && titleChange == false)
                {
                    start_text.pingpong = false;
                    start_text.opaque = false;
                    start_text.hold_opaque = true;
                    dark_bg.opaque = false;
                    title_name.opaque = false;

                    titleSlide = introsprt.gameObject.transform.localPosition.x;
                    titleChange = true;

                    introSource.Play();
                }

                if (titleChange && dark_bg.hold_opaque == false)
                {
                    if (titleSlide > 0.1f)
                    {
                        titleSlide -= Time.deltaTime + introsprt.gameObject.transform.localPosition.x/100f;
                        introsprt.gameObject.transform.localPosition = new Vector2(titleSlide, introsprt.gameObject.transform.localPosition.y);
                    }
                    else
                    {
                        titleSlide = 0.1f;
                        introsprt.gameObject.transform.localPosition = new Vector2(titleSlide, introsprt.gameObject.transform.localPosition.y);
                        title = false;
                    }
                }
            }
            else
            {
                if (started)
                {
                    if (introTime > 0)
                    {
                        introTime -= 0.05f;
                    }
                    else if (readyChange)
                    {
                        readyChange = false;
                        changeSlide = true;
                    }
                }

                if (changeSlide)
                {
                    changeSlide = false;
                    changeText = true;
                    intro_fade_txt.opaque = false;
                }

                if (changeText && !intro_fade_txt.hold_opaque)
                {
                    changeText = false;
                    changeSprite = true;
                    if (introCount != 0)
                    {
                        intro_fade_sprt.opaque = false;
                    }
                    else
                    {
                        changeReplace = true;
                    }
                }

                if (changeSprite && (!intro_fade_sprt.hold_opaque || changeReplace))
                {
                    changeSprite = false;
                    if (!readyChange)
                    {
                        nextIntro();
                    }
                }

                if (changeSpriteBack && intro_fade_sprt.hold_opaque)
                {
                    changeSpriteBack = false;
                    changeTextBack = true;
                    intro_fade_txt.opaque = true;
                }
                if (changeTextBack && intro_fade_txt.hold_opaque)
                {
                    changeTextBack = false;
                    introTime = 90f;
                    changeSlide = false;
                    readyChange = true;
                }
                if (introCount == 3 && !back_block.hold_opaque)
                {
                    introducing = false;
                    player_control.frozen = false;
                    back_block.gameObject.SetActive(false);
                    intro_fade_sprt.gameObject.SetActive(false);
                    intro_fade_txt.gameObject.SetActive(false);
                }
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                ShowInv();
            }

            if (checkInv)
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
                else if (Input.GetKeyDown(KeyCode.Space))
                {
                    if (!useItem)
                    {
                        if (!text_control.inspected && inspectDelay == 0)
                        {
                            InteractWith(inv_control.GetItemInteract(), false);
                            inspectDelay = 2;
                        }
                        else
                        {
                            text_control.ClearText();
                        }
                    }
                }
            }
            else if (choosing)
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
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (introducing)
            {
                if (!started)
                {
                    started = true;
                }
                else
                {
                    introTime = 0;
                }
            }
            else if (!checkInv)
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
                            /*
                             * interact with item immediately without using the menu if it is the only item within range
                             * disabled because it felt janky and weird, having the menu consistently pop up feels more natural
                            else if (interactables.Count == 1)
                            {
                                interact_with(interactables[0].GetCom ponent<Interactable>());
                            }
                            */
                            else
                            {
                                player_control.frozen = true;
                                choosing = true;
                                text_control.inspect_menu_object.SetActive(true);
                                text_control.ClearOptions();

                                for (int n = 0; n < interactables.Count; n++)
                                {
                                    Interactable currentInteract = interactables[n].GetComponent<Interactable>();
                                    options.Add(text_control.create_option(currentInteract.interact_name, currentInteract.GetInteractType()));
                                }

                                choose_count = 0;
                                options[0].GetComponent<choice>().select();
                            }
                        }
                        else
                        {
                            text_control.ClearText();
                        }
                    }
                }
                else
                {
                    Interactable interactWith = interactables[choose_count].GetComponent<Interactable>();

                    if ((interactWith.useInventory && !interactWith.observe && interactWith.interact) && !checkInv)
                    {
                        ShowInv();
                        useItem = true;
                    }
                    else
                    {
                        InteractWith(interactWith, false);
                    }
                }
            }
            else if (useItem) //if (options.Count > 0)
            {
                ShowInv();
                Debug.Log(interactables[choose_count].GetComponent<Interactable>());
                InteractWith(interactables[choose_count].GetComponent<Interactable>(), true);
                useItem = false;
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
        }

        if (text_control.inspecting)
        {
            if (player_control.input.magnitude > 0.8)
            {
                text_control.ClearText();
            }
        }

        if (moving && fade_control.hold_opaque)
        {
            player.transform.position = move_to;
            fade_control.opaque = false;
            finished_moving = true;
        }
        else if (finished_moving && !fade_control.hold_opaque)
        {
            finished_moving = false;
            moving = false;
            player_control.frozen = false;
        }
    }

    void InteractWith(Interactable toInteractWith, bool item)
    {
        UseInteractable(toInteractWith, item);
        text_control.ClearOptions();
        options.Clear();
        text_control.inspect_menu_object.SetActive(false);
        if (!toInteractWith.door)
        {
            player_control.frozen = false;
        }
        else if (toInteractWith.locked)
        {
            player_control.frozen = false;
        }
        choosing = false;
    }

    void ShowInv()
    {
        if (inv_control.invCount > 0)
        {
            checkInv = !checkInv;

            inv_control.GetComponent<Inventory>().ToggleInv();

            selectedItem = inv_control.GetItem();

            if (!checkInv)
            {
                player_control.frozen = false;
            }
        }
    }

    void UseInteractable(Interactable target, bool item)
    {
        selectedItem = inv_control.GetItem();
        string selectedItemName = null;

        if (item)
        {
            selectedItemName = selectedItem != null ? selectedItem.itemName : null;
            Debug.Log("Used " + selectedItemName + " item on " + target.name);
        }

        else
        {
            Debug.Log("Interacted with " + target.name);
        }

        target.interaction(this, selectedItemName, story_control);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void Escape()
    {
        if (checkInv)
        {
            if (options.Count > 0)
            {
                ShowInv();
            }
        }
        else if (choosing)
        {
            text_control.ClearOptions();
            options.Clear();
            text_control.inspect_menu_object.SetActive(false);
            choosing = false;
            player_control.frozen = false;
        }
        else
        {
            if (escapeMenu.activeSelf)
            {
                escapeMenu.SetActive(false);
                player_control.frozen = false;
                Time.timeScale = 1;
            }
            else
            {
                escapeMenu.SetActive(true);
                player_control.frozen = true;
                Time.timeScale = 0;
            }
        }
    }

    void nextIntro()
    {
        if (introCount < 3)
        {
            introsprt.sprite = introsprts[introCount];
            introtxt.text = introtxts[introCount].text;
            introtxt.gameObject.transform.localPosition = introtxts[introCount].gameObject.transform.localPosition;

            introCount++;

            changeSpriteBack = true;
            intro_fade_sprt.opaque = true;
            changeReplace = false;
        }
        else
        {
            back_block.opaque = false;
            StartCoroutine(FadeOut());
        }
    }

    public void ChangeSurface(SurfaceType newSurface)
    {
        if (roomType != newSurface)
        {
            roomType = newSurface;

            List<AudioClip> newAudioMix = new List<AudioClip>();

            switch (newSurface)
            {
                case SurfaceType.EXTERIOR:
                    newAudioMix.AddRange(ambientOutside);
                    break;
                case SurfaceType.HARDWOOD:
                    newAudioMix.AddRange(drone);
                    newAudioMix.AddRange(ambientInside);
                    break;
                case SurfaceType.FLOORBOARD:
                    newAudioMix.AddRange(drone);
                    newAudioMix.AddRange(ambientInside);
                    newAudioMix.AddRange(staticClip);
                    break;
                case SurfaceType.TILE:
                    newAudioMix.AddRange(drone);
                    //newAudioMix.AddRange(ambientInside);
                    newAudioMix.AddRange(kitchen);
                    break;
                case SurfaceType.CEMENT:
                    newAudioMix.AddRange(drone);
                    //newAudioMix.AddRange(ambientInside);
                    newAudioMix.AddRange(metal);
                    break;
                default: break;
            }

            audioControl.ChangeClip(newAudioMix);
        }
    }

    public void PlayClip(List<AudioClip> play)
    {
        audioControl.PlayMultipleClips(play);
    }

    public void AddDisabledInteract(Interactable interactToAdd)
    {
        disabledInteracts.Add(interactToAdd);
        interactToAdd.enabled = false;
    }

    public void ReEnableInteracts()
    {
        foreach (Interactable interactToReEnable in disabledInteracts)
        {
            interactToReEnable.enabled = true;
        }
    }

    public void PlayTrill()
    {
        audioControl.PlaySingleClip(inspectTrill[inspectTrillCount]);
        inspectTrillCount++;
        if (inspectTrillCount >= inspectTrill.Count)
        {
            inspectTrillCount = 0;
        }
    }

    public IEnumerator FadeOut()
    {
        float fadeValue = 0;

        while (fadeValue < 1)
        {
            introSource.volume = Mathf.Lerp(1, 0, fadeValue);
            fadeValue += 0.01f * Time.deltaTime;

            yield return null;
        }

        yield break;
    }
}
