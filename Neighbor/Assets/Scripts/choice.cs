using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class choice : MonoBehaviour
{
    public SpriteRenderer symbol;

    public void select()
    {
        GetComponent<TextMeshProUGUI>().color = new Vector4(1, 0, 0, 1);
        symbol.color = new Vector4(1, 0, 0, 1);
    }

    public void deselect()
    {
        GetComponent<TextMeshProUGUI>().color = new Vector4(1, 1, 1, 1);
        symbol.color = new Vector4(1, 1, 1, 1);
    }
}
