using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    [System.NonSerialized] public bool show = true;
    [System.NonSerialized] public bool hold;

    private Vector4 init_color;
    private float alpha;
    private SpriteRenderer[] sprite;

    private float destination = -20.0f;

    public string description;

    void Start()
    {
        sprite = GetComponentsInChildren<SpriteRenderer>();
    }

    void Update()
    {
        if (show != hold)
        {
            transform.position = new Vector2(Mathf.Lerp(transform.position.x, destination, alpha), transform.position.y);

            if (show)
            {
                if (alpha < 1f)
                {
                    alpha += 0.02f;
                }
                else
                {
                    alpha = 1f;
                    hold = show;
                    destination = -20.0f;
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
                    hold = show;
                    destination = 0.0f;
                    transform.position = new Vector2(transform.position.x + 20.0f, transform.position.y);
                }
            }
            for (int c = 0; c<sprite.Length; c++)
            {
                sprite[c].color = new Vector4(init_color[0], init_color[1], init_color[2], alpha);
            }
        }
    }
}
