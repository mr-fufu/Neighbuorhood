using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowFloat : MonoBehaviour
{
    public int vertical;
    public int horizontal;

    private Vector2 position;
    private float hor_disp = 0;
    private float ver_disp = 0;

    void Start()
    {
        position = new Vector2(transform.position.x, transform.position.y);
    }

    void Update()
    {
        hor_disp = Mathf.PingPong(Time.time * 8f, 6f);
        ver_disp = Mathf.PingPong(Time.time * 4f, 3f);
        transform.position = new Vector2(position.x + hor_disp * horizontal, position.y + ver_disp * vertical);
    }
}
