using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsoCharacter : MonoBehaviour
{
    public static readonly string[] staticDirections =
    {
        "S_NE",
        "S_NW",
        "S_SW",
        "S_SE",
    };

    public static readonly string[] dynamicDirections =
    {
        "D_NE",
        "D_NW",
        "D_SW",
        "D_SE",
    };

    Animator anim;
    public int lastDir;

    void Start()
    {
        anim = GetComponent<Animator>();
        lastDir = 2;
        SetDirection(Vector2.zero);
    }

    public void SetDirection(Vector2 direction)
    {
        string[] dirArray = null;

        if (direction.magnitude < 0.01f)
        {
            dirArray = staticDirections;
        }
        else
        {
            dirArray = dynamicDirections;
            lastDir = DirectionToIndex(direction, 4);
        }

        anim.Play(dirArray[lastDir]);
    }

    public void SetDirectionStanding()
    {
        anim.Play(staticDirections[lastDir]);
    }

    public static int DirectionToIndex(Vector2 dir, int count)
    {
        Vector2 normDir = dir.normalized;

        float step = 360f / count;

        float half = step / 2;

        float angle = Vector2.SignedAngle(Vector2.up, normDir);

        angle += half;
        angle += 1;

        if (angle < 0)
        {
            angle += 360;
        }

        float stepCount = angle / step;

        return Mathf.FloorToInt(stepCount);
    }

}
