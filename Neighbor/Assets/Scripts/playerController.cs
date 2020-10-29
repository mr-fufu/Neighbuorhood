using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed;
    private IsoCharacter isoRend;
    [System.NonSerialized] public bool frozen;
    [System.NonSerialized] public Vector2 input;

    void Start()
    {
        isoRend = GetComponent<IsoCharacter>();
    }

    void FixedUpdate()
    {
        Vector2 current_position = GetComponent<Rigidbody2D>().position;
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        input = Vector2.ClampMagnitude(input, 1);

        if (!frozen)
        {
            Vector2 moveto = current_position + input * speed * Time.deltaTime;
            isoRend.SetDirection(input * speed);
            GetComponent<Rigidbody2D>().MovePosition(moveto);
        }
        else
        {
            isoRend.SetDirection(input * 0.0001f);
        }
    }
}
