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

    public List<Sprite> interactSprite;

    void Start()
    {
        inspect_container.SetActive(true);
        inspect_text = inspect_text_object.GetComponent<TextMeshProUGUI>();
        inspect_text_object.SetActive(false);
        inspect_menu_object.SetActive(false);
        initial_position = inspect_text_object.transform.localPosition;
    }

    public GameObject create_option(string option_name, string interactType)
    {
        GameObject option = Instantiate<GameObject>(inspect_options,inspect_menu_object.transform);

        string interactPrefix = "";
        SpriteRenderer symbolSprite = option.transform.GetChild(0).GetComponent<SpriteRenderer>();
        switch (interactType)
        {
            case "observe":
                symbolSprite.sprite = interactSprite[0];
                interactPrefix = "[Observe]";
                break;
            case "pickup":
                symbolSprite.sprite = interactSprite[1];
                interactPrefix = "[Pickup]";
                break;
            case "door":
                symbolSprite.sprite = interactSprite[2];
                interactPrefix = "[Open]";
                break;
            case "inventory":
                symbolSprite.sprite = interactSprite[3];
                interactPrefix = "[Use item on]";
                break;
            case "interact":
                symbolSprite.sprite = interactSprite[4];
                interactPrefix = "[Use]";
                break;
            case "unknown":
                symbolSprite.sprite = interactSprite[5];
                interactPrefix = "[???]";
                break;
            default:
                break;
        }

        option.GetComponent<TextMeshProUGUI>().SetText(interactPrefix + " " + option_name);

        return option;
    }

    public void ClearOptions()
    {
        foreach (Transform option in inspect_menu_object.transform)
        {
            GameObject.Destroy(option.gameObject);
        }
    }

    public void ShowTextFromObject(GameObject textObject)
    {
        if (textObject != null)
        {
            if (textObject.GetComponent<TextMeshPro>() != null)
            {
                ShowText(textObject.GetComponent<TextMeshPro>().text);
            }
        }
    }

    public void ShowText(string textToShow)
    {
        if (!inspected)
        {
            inspect_text.text = textToShow;
            inspecting = true;
            rise = 0f;
            life = 120f;
        }
        else
        {
            ClearText();
        }
    }

    public void ClearText()
    {
        if (alpha < 0.25f || life >= 119.0f)
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
        if (wait_to_clear && alpha >= 0.25f && life <= 119.0f)
        {
            ClearText();
            wait_to_clear = false;
        }

        if (life > 0)
        {
            life -= Time.deltaTime * 0.8f;
        }
        else
        {
            life = 0;
            if (inspecting && inspected)
            {
                ClearText();
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
