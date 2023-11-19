using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFloat : MonoBehaviour
{
    public int vertical;
    public int horizontal;
    public bool customSpeed;
    public float speed;

    private Vector2 position;
    private float hor_disp = 0;
    private float ver_disp = 0;

    void Start()
    {
        if (!customSpeed)
        {
            speed = 1.0f;
        }
        position = new Vector2(transform.localPosition.x, transform.localPosition.y);
    }

    void Update()
    {
        hor_disp = Mathf.PingPong(Time.time * speed * 8f, 6f);
        ver_disp = Mathf.PingPong(Time.time * speed * 4f, 3f);

        transform.localPosition = new Vector2(position.x + hor_disp * horizontal, position.y + ver_disp * vertical);
    }
}
