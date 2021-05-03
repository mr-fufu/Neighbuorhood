using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    public GameObject selfLocation;
    public GameObject gotoLocation;
    public GameObject pointerArrow;
    public bool usePointer;
    public SurfaceType toLocationSurface;

    private void Start()
    {
        showArrow(false);
    }

    public void showArrow(bool show)
    {
        if (usePointer)
        {
            pointerArrow.SetActive(show);
        }
    }

    public void useDoor(GameController game_controller)
    {
        game_controller.movePlayerTo(gotoLocation);
        game_controller.ChangeSurface(toLocationSurface);
    }
}
