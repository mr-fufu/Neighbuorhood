using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FadeController : MonoBehaviour
{
    public GameObject fade_obj;
    public bool text;
    public bool opaque;
    public bool pingpong;

    private Vector4 init_color;
    private float alpha;
    [System.NonSerialized] public bool hold_transparency;
    private SpriteRenderer sprite;
    private TextMeshProUGUI text_comp;
    private TextMeshPro text_comp_alt;
    private bool alt;

    void Start()
    {
        fade_obj.SetActive(true);

        init_color = new Vector4(0, 0, 0, 0);

        if (text)
        {
            if (fade_obj.GetComponent<TextMeshProUGUI>() != null)
            {
                alt = false;
                text_comp = fade_obj.GetComponent<TextMeshProUGUI>();
                init_color = text_comp.color;
            }
            else
            {
                alt = true;
                text_comp_alt = fade_obj.GetComponent<TextMeshPro>();
                init_color = text_comp_alt.color;
            }
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
            if (!alt)
            {
                text_comp.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
            }
            else
            {
                text_comp_alt.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
            }
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
                if (!alt)
                {
                    text_comp.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
                }
                else
                {
                    text_comp_alt.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
                }
            }
            else
            {
                sprite.color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
            }
        }
        else if (pingpong)
        {
            opaque = !opaque;
        }
    }
}
