using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeController : MonoBehaviour
{
    public GameObject black_obj;
    public bool black;

    private float alpha;
    [System.NonSerialized] public bool hold_black;
    private SpriteRenderer sprite;

    void Start()
    {
        black_obj.SetActive(true);
        sprite = black_obj.GetComponent<SpriteRenderer>();
        alpha = 0;
        sprite.color = new Vector4(0, 0, 0, alpha);
    }

    void Update()
    {
        if (black != hold_black)
        {
            if (black)
            {
                if (alpha < 1f)
                {
                    alpha += 0.02f;
                }
                else
                {
                    alpha = 1f;
                    hold_black = black;
                }
            }
            else
            {
                if (alpha > 0)
                {
                    alpha -= 0.02f;
                }
                else
                {
                    alpha = 0f;
                    hold_black = black;
                }
            }
            sprite.color = new Vector4(0, 0, 0, alpha);
        }
    }
}
