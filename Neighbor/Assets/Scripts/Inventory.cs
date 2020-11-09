using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject container_template;
    private List<GameObject> container = new List<GameObject>();

    public GameObject selector;

    public Transform show_location;
    public Transform hide_location;

    private float slide;

    public int con_index = 0;
    public int inv_index = 0;

    public int inv_count = 0;

    public bool inventory_visible;
    public bool inventory_hold;
    private bool inv_transition;

    void Start()
    {
        GameObject init_cont = Instantiate(container_template, transform);
        container.Add(init_cont);
    }

    void Update()
    {
        if (container[con_index].transform.childCount > 0)
        {
            selector.transform.position = container[con_index].transform.GetChild(inv_index).position;
        }

        if (inventory_hold != inventory_visible)
        {
            inv_transition = true;

            if (inventory_visible)
            {
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, show_location.position.x, slide), transform.position.y);
            }
            else
            {
                transform.position = new Vector2(Mathf.Lerp(transform.position.x, hide_location.position.x, slide), transform.position.y);
            }

            slide += 0.5f * Time.deltaTime;

            if (slide >= 1.0f)
            {
                slide = 0.0f;
                inventory_hold = inventory_visible;
            }
        }
        else
        {
            inv_transition = false;
        }
    }

    public void inv_move(int v, int h)
    {
        Debug.Log(v);
        Debug.Log(h);

        if (inv_index%2 == 0)
        {
            if (h > 0)
            {
                inv_index += 2 * v + h;
            }
            else
            {
                con_Move(false);
            }
        }
        else
        {
            if (h < 0)
            {
                inv_index += 2 * v - Mathf.Abs(h);
            }
            else
            {
                con_Move(true);
            }
        }
    }

    public void con_Move(bool next)
    {
        if (container.Count > 1)
        {
            container[con_index].GetComponent<Container>().show = false;

            if (next)
            {
                if (con_index < container.Count)
                {
                    container[con_index + 1].GetComponent<Container>().show = true;
                    con_index += 1;
                }
                else
                {
                    container[0].GetComponent<Container>().show = true;
                    con_index = 0;
                }
            }
            else
            {
                if (con_index > container.Count)
                {
                    container[con_index - 1].GetComponent<Container>().show = true;
                    con_index -= 1;
                }
                else
                {
                    container[container.Count].GetComponent<Container>().show = true;
                    con_index = container.Count;
                }
            }
        }
    }

    public void toggleInv()
    {
        if (inv_transition == false)
        {
            inventory_visible = !inventory_visible;
        }
        else
        {
            inventory_visible = !inventory_visible;
            inventory_hold = !inventory_hold;
            slide = 1.0f - slide;
        }
    }

    public void addItem(GameObject new_inv)
    {
        if (inv_count < 8)
        {
            GameObject new_item = GameObject.Instantiate(new_inv, container[container.Count - 1].transform);
            new_item.SetActive(true);
            inv_count++;
        }
        else
        {
            GameObject new_cont = Instantiate(container_template, transform);
            container.Add(new_cont);
            GameObject new_item = GameObject.Instantiate(new_inv, container[container.Count - 1].transform);
            new_item.SetActive(true);
            inv_count = 1;
        }
    }

    public void removeItem()
    {
        Destroy(container[con_index].transform.GetChild(inv_index).gameObject);
    }
}
