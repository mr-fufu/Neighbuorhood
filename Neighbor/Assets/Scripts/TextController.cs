using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextController : MonoBehaviour
{
    public GameObject inspect_container;
    public GameObject inspect_text_object;
    public GameObject inspect_menu_object;
    public GameObject inspect_options;

    private TextMeshProUGUI inspect_text;
    public bool inspecting = false;
    public bool inspected;
    private float rise;
    private Vector4 color;
    private float alpha;
    private Vector2 initial_position;
    private float life;
    private bool wait_to_clear;

    void Start()
    {
        inspect_container.SetActive(true);
        inspect_text = inspect_text_object.GetComponent<TextMeshProUGUI>();
        inspect_text_object.SetActive(false);
        inspect_menu_object.SetActive(false);
        initial_position = inspect_text_object.transform.localPosition;
    }

    public GameObject create_option(string option_name)
    {
        GameObject option = Instantiate<GameObject>(inspect_options,inspect_menu_object.transform);
        Debug.Log(option);
        option.GetComponent<TextMeshProUGUI>().SetText(option_name);
        return option;
    }

    public void clear_options()
    {
        foreach (Transform option in inspect_menu_object.transform)
        {
            GameObject.Destroy(option.gameObject);
        }
    }

    public void show_text(GameObject text)
    {
        if (text != null)
        {
            if (text.GetComponent<TextMeshPro>() != null)
            {
                if (!inspected)
                {
                    inspect_text.text = text.GetComponent<TextMeshPro>().text;
                    inspecting = true;
                    rise = 0f;
                    life = 120f;
                }
            }
            else
            {
                clear_text();
            }
        }
        else
        {
            clear_text();
        }
    }

    public void clear_text()
    {
        if (alpha < 0.25f)
        {
            wait_to_clear = true;
        }
        else
        {
            if (inspecting && !inspected)
            {
                inspected = true;
                inspecting = false;
            }
            else if (inspecting && inspected)
            {
                inspecting = false;
            }
        }
    }

    void Update()
    {
        if (wait_to_clear && alpha >= 0.25f)
        {
            clear_text();
            wait_to_clear = false;
        }

        if (life > 0)
        {
            life -= 0.2f;
        }
        else
        {
            life = 0;
            if (inspecting && inspected)
            {
                    clear_text();
            }
        }

        if (inspecting != inspected)
        {
            if (inspecting)
            {
                inspect_text_object.SetActive(true);

                if (alpha < 1f)
                {
                    alpha += 0.02f;
                    rise += 0.01f;
                }
                else
                {
                    alpha = 1f;
                    inspected = inspecting;
                }
            }
            else
            {
                if (alpha > 0)
                {
                    alpha -= 0.02f;
                    rise += 0.01f;
                }
                else
                {
                    alpha = 0f;
                    inspected = inspecting;
                    inspect_text_object.SetActive(false);
                }
            }
            inspect_text.color = new Vector4(1, 1, 1, alpha);
            inspect_text_object.transform.localPosition = new Vector2(initial_position.x, initial_position.y + rise);
        }
    }
}
