using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideController : MonoBehaviour
{
    private bool slideShowing;
    public FadeController dark;
    public SpriteRenderer slide;
    public GameObject background;

    private List<Sprite> slideDeck;
    private List<float> slideDeckTiming;

    private float timer = 0;
    private int slideCount;
    private bool fadeState;
    private bool fadeStateHold;

    void Update()
    {
        if (slideShowing)
        {
            Transition(fadeState);
        }
    }

    public void ShowSlides(List<Sprite> slidesToShow, List<float> slideTimings)
    {
        if (!slideShowing)
        {
            slideShowing = !slideShowing;

            slideDeck = slidesToShow;
            slideDeckTiming = slideTimings;

            slideCount = 0;
            fadeState = true;
        }
    }

    void NextSlide()
    {
        if (slideCount == 0)
        {
            slide.gameObject.SetActive(true);
            background.SetActive(true);
        }

        if (slideCount < slideDeck.Count)
        {
            slide.sprite = slideDeck[slideCount];
            timer = slideDeckTiming[slideCount];
            slideCount++;
        }
        else
        {
            slideShowing = false;
            dark.opaque = false;
            slide.gameObject.SetActive(false);
            background.SetActive(false);
        }
    }

    void Transition(bool fadeToState)
    {
        if (timer > 0 && !fadeStateHold && fadeState) 
        {
            timer -= Time.deltaTime;
        }
        else
        {
            dark.opaque = fadeToState;
        }

        if (dark.hold_opaque == fadeToState)
        {
            fadeStateHold = fadeToState;
            if (fadeToState)
            {
                NextSlide();
            }
            fadeState = !fadeState;
        }
    }
}
