using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeController : MonoBehaviour
{
    public GameObject fade_obj;
    public bool text;
    public bool opaque;

    private Vector4 init_color;
    private float alpha;
    [System.NonSerialized] public bool hold_transparency;
    private SpriteRenderer sprite;
    private TextMeshProUGUI text_comp;

    void Start()
    {
        fade_obj.SetActive(true);

        init_color = new Vector4(0, 0, 0, 0);

        if (text)
        {
            text_comp = fade_obj.GetComponent<TextMeshProUGUI>();
            init_color = text_comp.color;
        }
        else
        {
            sprite = fade_obj.GetComponent<SpriteRenderer>();
            init_color = sprite.color;
        }



        if (opaque)
        {
            alpha = 1;
        }
        else
        {
            alpha = 0;
        }

        if (text)
        {
            text_comp.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
        }
        else
        {
            sprite.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
        }
    }

    void Update()
    {
        if (opaque != hold_transparency)
        {
            if (opaque)
            {
                if (alpha < 1f)
                {
                    alpha += 0.02f;
                }
                else
                {
                    alpha = 1f;
                    hold_transparency = opaque;
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
                    hold_transparency = opaque;
                }
            }
            if (text)
            {
                text_comp.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
            }
            else
            {
                sprite.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
            }
        }
    }
}
