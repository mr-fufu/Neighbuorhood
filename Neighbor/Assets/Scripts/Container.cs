using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : MonoBehaviour
{
    public bool show;
    public bool hold;

    public float alpha;
    public List<SpriteRenderer> sprite;
    private SpriteRenderer selfSprite;

    public float destination = 0.0f;
    public float slide = 0.0f;

    public int no_items;

    void Start()
    {
        selfSprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        for (int c = 0; c < sprite.Count; c++)
        {
            sprite[c].color = new Vector4(1, 1, 1, alpha);
        }

        selfSprite.color = new Vector4(1, 1, 1, alpha);

        if (show != hold)
        {
            transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, destination, slide), transform.localPosition.y);

            if (show)
            {
                if (alpha < 1f)
                {
                    slide += 0.05f;
                    alpha += 0.05f;
                }
                else
                {
                    slide = 0.0f;
                    alpha = 1f;
                    hold = show;
                }
            }
            else
            {
                if (alpha > 0f)
                {
                    slide += 0.05f;
                    alpha -= 0.05f;
                }
                else
                {
                    slide = 0.0f;
                    alpha = 0f;
                    hold = show;
                }
            }
        }
    }

    public void add_item(SpriteRenderer new_sprite)
    {
        sprite.Add(new_sprite);
        new_sprite.color = new Vector4(1, 1, 1, 0);
        no_items++;
    }

    public void remove_item(int sprite_no)
    {
        sprite.Remove(sprite[sprite_no]);
        no_items--;
    }
}
