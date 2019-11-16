using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Commands;
using System;

public class Inventory : MonoBehaviour
{
    //how close does an object need to be before it can be grabbed?
    public float grabRange = 10.0f;

    private ILookup<string, Item> items;

    //this is the item that the character is currently using, or null if none
    private Item current = null;


    public ICommand UseCurrentItemCmd = NullCommand.Instance;
    public ICommand NextItemCmd = NullCommand.Instance;
    public ICommand PrevItemCmd = NullCommand.Instance;
    public ICommand UnselectItemCmd = NullCommand.Instance;
    public ICommand PickUpItemCmd = NullCommand.Instance;
    public List<ICommand> ChangeToItemCmds = Enumerable.Repeat(NullCommand.Instance as ICommand, 9).ToList();

    public Inventory()
    {
        UseCurrentItemCmd = new ActionCommand("Use the Current Item", UseCurrentItem);
        NextItemCmd = new ActionCommand("Select Next Item", SelectNextItem);
        PrevItemCmd = new ActionCommand("select Previous Item", SelectPrevItem);
        UnselectItemCmd = new ActionCommand("Deselect All Items", () => current = null);
        PickUpItemCmd = new ActionCommand("Pick up the closest item", PickUpItem);
        for (int i = 0; i < ChangeToItemCmds.Count; i++)
        {
            ChangeToItemCmds[i] = UnselectItemCmd as ICommand;
        }
    }

    public void Start()
    {
        UpdateItems();
    }

    public void UpdateItems()
    {
        var itemList = GetComponentsInChildren<Item>(true);
        foreach (var item in itemList)
        {
            item.Deactivate();
        }

        items = itemList.ToLookup(x => x.identifier);
        for (int i = 0; i < items.Count && i < ChangeToItemCmds.Count; i++)
        {
            IGrouping<string, Item> group = items.ElementAt(i);

            //Note that pulling this value out of the group outside of the lambda is very important.
            // using group.First() inside the lambda means that the lambda must maintain a reference to the whole group
            // as long as it lives.
            // Doing it this way means that the lambda only holds a reference to the item itself.
            Item firstInGroup = group.First();
            ICommand newCommand =
            ChangeToItemCmds[i] = new ActionCommand("Select Item: " + group.Key, () => current = firstInGroup) as ICommand;
        }
    }

    //display the objects on hand
    public void OnGUI()
    {
        if (items.Count > 0)
        {
            GUILayout.BeginArea(new Rect(10, 80, 60, Screen.height - 80 - 20));
            GUILayout.BeginVertical(GUI.skin.box);

            foreach (IGrouping<string, Item> curItemGroup in items)
            {
                //choose the icon to display
                GUIContent tmp;
                if (current != null && curItemGroup.Key == current.identifier)
                {
                    tmp = new GUIContent(curItemGroup.Count().ToString(), curItemGroup.First().icon_selected);
                }
                else
                {
                    tmp = new GUIContent(curItemGroup.Count().ToString(), curItemGroup.First().icon_unselected);
                }

                //display it
                GUILayout.Label(tmp);
            }
            GUILayout.EndVertical();
            GUILayout.EndArea();
        }
    }

    public void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            UseCurrentItemCmd.Execute();
        }
        if (Input.GetButton("Fire2"))
        {
            PickUpItemCmd.Execute();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            NextItemCmd.Execute();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            PrevItemCmd.Execute();
        }

        for (int i = 0; i < ChangeToItemCmds.Count; i++)
        {
            if (Input.GetKeyUp((i + 1).ToString()))
            {
                ChangeToItemCmds[i].Execute();
            }
        }
    }





    private void SelectNextItem()
    {
        if (current == null || !items.Contains(current.identifier))
        {
            if (items.Count <= 0)
            {
                current = null;
            }
            else
            {
                current = items.First().First();
            }
        }
        else
        {
            var i = items.Select(x => x.Key).ToList().IndexOf(current.identifier);
            i += 1;
            if (i >= items.Count)
            {
                current = null;
            }
            else
            {
                current = items.ElementAt(i).First();
            }
        }
    }

    private void SelectPrevItem()
    {
        if (current == null || !items.Contains(current.identifier))
        {
            current = items.Last().First();
        }
        else
        {
            var i = items.Select(x => x.Key).ToList().IndexOf(current.identifier);
            i -= 1;
            if (i < 0)
            {
                current = null;
            }
            else
            {
                current = items.ElementAt(i).First();
            }
        }
    }

    private void UseCurrentItem()
    {
        if (current != null)
        {
            string prevIdentifier = current.identifier;
            current.UseItem();
            current.transform.parent = null;
            current.inInventory = false;

            UpdateItems();
            if (items.Contains(prevIdentifier))
            {
                current = items[prevIdentifier].First();
            }
            else
            {
                current = null;
            }
        }
    }

    private void PickUpItem()
    {
        Collider[] inRange = Physics.OverlapSphere(transform.position, grabRange, 1 << LayerMask.NameToLayer("Items"));

        Item nearObject = inRange.OrderBy(x => Vector3.Distance(x.transform.position, transform.position))
                                 .Select(x => x.GetComponent<Item>())
                                 .FirstOrDefault(x => x != null);

        //add it to the inventory
        if (nearObject != null)
        {
            nearObject.Deactivate();
            nearObject.transform.parent = this.transform;
            nearObject.inInventory = true;
            UpdateItems();
        }
    }
}
