using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerController : MonoBehaviour
{
    public float speed;
    private IsoCharacter isoRend;
    public bool frozen;
    [System.NonSerialized] public Vector2 input;
    private Rigidbody2D selfBody;
    private RigidbodyConstraints2D selfConstraints;

    void Start()
    {
        selfBody = GetComponent<Rigidbody2D>();
        selfConstraints = selfBody.constraints;
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
            selfBody.constraints = selfConstraints;
        }
        else
        {
            isoRend.SetDirection(input * 0.00001f);
            selfBody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }
}
