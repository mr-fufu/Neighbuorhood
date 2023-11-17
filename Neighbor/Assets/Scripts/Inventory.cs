using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject container_template;
    public List<GameObject> container = new List<GameObject>();
    public List<GameObject> startingItems;

    public GameObject selector;

    public Transform show_location;
    public Transform hide_location;

    private float slide;

    private int conIndex = 0;
    private int invIndex = 0;

    public int invCount = 0;

    public bool inventory_visible;
    public bool inventory_hold;
    private bool inv_transition;
    public bool transit;

    void Start()
    {
        //gameObject.transform.localPosition = new Vector2((Screen.width / 2) + 20.0f, 0);
        GameObject init_cont = Instantiate(container_template, transform);
        init_cont.transform.localPosition = new Vector2(0, 0);
        init_cont.GetComponent<Container>().alpha = 1.0f;
        init_cont.GetComponent<Container>().show = true;
        container.Add(init_cont);

        foreach (GameObject startItem in startingItems)
        {
            addItem(startItem);
        }
    }

    void Update()
    {
        if (container[conIndex].transform.childCount > 0)
        {
            if (!transit)
            {
                selector.transform.position = container[conIndex].transform.GetChild(invIndex).position;
                //equipped.transform.position = container[equipConIndex].transform.GetChild(equipInvIndex).position;
            }
            else
            {
                if (container[conIndex].GetComponent<Container>().hold == false)
                {
                    transit = false;
                }
            }
        }

        if (inventory_hold != inventory_visible)
        {
            inv_transition = true;

            if (inventory_visible)
            {
                transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, show_location.localPosition.x, slide), show_location.localPosition.y);
            }
            else
            {
                transform.localPosition = new Vector2(Mathf.Lerp(transform.localPosition.x, hide_location.localPosition.x, slide), hide_location.localPosition.y);
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
        int totalItems = container[conIndex].GetComponent<Container>().no_items;

        if (h != 0)
        {
            if (totalItems % 2 == 1 && invIndex == totalItems - 1)
            {
                if (h > 0)
                {
                    if (conIndex < container.Count - 1)
                    {
                        con_Move(true);
                    }
                }
                else
                {
                    if (conIndex > 0)
                    {
                        con_Move(false);
                    }
                }
            }
            else
            {
                if (invIndex % 2 == 0)
                {
                    if (h > 0)
                    {
                        invIndex += h;
                    }
                    else
                    {
                        if (conIndex > 0)
                        {
                            con_Move(false);
                        }
                    }
                }
                else
                {
                    if (h < 0)
                    {
                        invIndex += -Mathf.Abs(h);
                    }
                    else
                    {
                        if (conIndex < container.Count - 1)
                        {
                            con_Move(true);
                        }
                    }
                }
            }
        }
        else if (v!= 0)
        {


            int adjust = (invIndex % 2 == 0 ? 1 : -1);

            if (v < 0 && invIndex < 2)
            { 
                invIndex += (totalItems + (totalItems % 2 * adjust)) + 2 * v;
            }
            else if (v > 0 && invIndex > container[conIndex].GetComponent<Container>().no_items - 3)
            {
                invIndex += -(totalItems + (totalItems % 2 * adjust)) + 2 * v;
            }
            else
            {
                invIndex += 2 * v;
            }
        }
    }

    public void con_Move(bool next)
    {
        if (container.Count > 1)
        {
            transit = true;

            if (next)
            {
                container[conIndex].GetComponent<Container>().destination = -40.0f;
                container[conIndex].GetComponent<Container>().show = false;

                if (conIndex == container.Count - 2)
                {
                    if (invIndex >= invCount)
                    {
                        invIndex = invCount - 1;
                    }
                }

                if (conIndex < container.Count)
                {
                    container[conIndex + 1].gameObject.transform.localPosition = new Vector2(40, container[conIndex + 1].gameObject.transform.localPosition.y);
                    container[conIndex + 1].GetComponent<Container>().destination = 0.0f;
                    container[conIndex + 1].GetComponent<Container>().show = true;
                    conIndex += 1;
                }
                else
                {
                    container[0].gameObject.transform.localPosition = new Vector2(40, container[0].gameObject.transform.localPosition.y);
                    container[0].GetComponent<Container>().destination = 0.0f;
                    container[0].GetComponent<Container>().show = true;
                    conIndex = 0;
                }
            }
            else
            {
                container[conIndex].GetComponent<Container>().destination = 40.0f;
                container[conIndex].GetComponent<Container>().show = false;

                if (conIndex == 0)
                {
                    container[container.Count - 1].gameObject.transform.localPosition = new Vector2(-40, container[container.Count - 1].gameObject.transform.localPosition.y);
                    container[container.Count - 1].GetComponent<Container>().destination = 0.0f;
                    container[container.Count - 1].GetComponent<Container>().show = true;
                    conIndex = container.Count - 1;
                }
                else
                {
                    container[conIndex - 1].gameObject.transform.localPosition = new Vector2(-40, container[conIndex - 1].gameObject.transform.localPosition.y);
                    container[conIndex - 1].GetComponent<Container>().destination = 0.0f;
                    container[conIndex - 1].GetComponent<Container>().show = true;
                    conIndex -= 1;
                }
            }
        }
    }

    public void ToggleInv()
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

    public void addItem(GameObject itemToAdd)
    {
        if (invCount < 8)
        {
            GameObject new_item = GameObject.Instantiate(itemToAdd, container[container.Count - 1].transform);
            container[container.Count - 1].GetComponent<Container>().addToContainer(new_item.GetComponent<SpriteRenderer>());
            new_item.SetActive(true);
            invCount++;
            new_item.GetComponent<SpriteRenderer>().sortingOrder = 5010;
        }
        else
        {
            GameObject new_cont = Instantiate(container_template, transform); 
            new_cont.GetComponent<Container>().show = false;
            new_cont.GetComponent<Container>().destination = 0.0f;
            new_cont.transform.localPosition = new Vector2(20, 0);
            container.Add(new_cont);
            GameObject new_item = GameObject.Instantiate(itemToAdd, container[container.Count - 1].transform);
            container[container.Count - 1].GetComponent<Container>().addToContainer(new_item.GetComponent<SpriteRenderer>());
            new_item.SetActive(true);
            invCount = 1;
            new_item.GetComponent<SpriteRenderer>().sortingOrder = 5010;
        }
    }

    public void RemoveItem()
    {
        invCount--;

        container[conIndex].GetComponent<Container>().removeFromContainer(invIndex);
        Destroy(container[conIndex].transform.GetChild(invIndex).gameObject);

        if (conIndex < container.Count - 1)
        {
            for (int con_no = conIndex; con_no < container.Count; con_no++)
            {
                container[conIndex + 1].transform.GetChild(0).SetParent(container[conIndex].transform);
            }
        }
    }

    public void RemoveItemByIndex(int removeConIndex, int removeInvIndex)
    {
        invCount--;

        container[removeConIndex].GetComponent<Container>().removeFromContainer(removeInvIndex);
        Destroy(container[removeConIndex].transform.GetChild(removeInvIndex).gameObject);

        if (removeConIndex < container.Count)
        {
            for (int con_no = removeConIndex; con_no < container.Count; con_no++)
            {
                container[removeConIndex + 1].transform.GetChild(0).SetParent(container[removeConIndex].transform);
            }
        }
    }

    public void RemoveSpecificItem(Sprite itemToRemove)
    {
        int spriteConIndex = 0;

        foreach (GameObject searchCon in container)
        {
            int spriteInvIndex = searchCon.GetComponent<Container>().searchContainer(itemToRemove);

            if (spriteInvIndex != 9)
            {
                RemoveItemByIndex(spriteConIndex, spriteInvIndex);
                break;
            }
            else
            {
                spriteConIndex++;
            }
        }
    }

    public InventoryItem GetItem()
    {
        if (invCount > 0)
        {
            return container[conIndex].transform.GetChild(invIndex).GetComponent<InventoryItem>();
        }
        else
        {
            return null;
        }
    }

    public Interactable GetItemInteract()
    {
        Interactable returnInteract = container[conIndex].transform.GetChild(invIndex).GetComponent<Interactable>();
        if (returnInteract != null)
        {
            return returnInteract;
        }
        else
        {
            return null;
        }
    }
}
