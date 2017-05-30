using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    protected GameObject gameObject;
    protected string name;
    protected int amount;
    protected int indexInInventory;

    public GameObject GameObject
    {
        get { return gameObject; }
        set { gameObject = value; }
    }

    public string Name
    {
        get { return name; }
        set
        {
            name = value;
        }
    }

    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public int IndexInInventory
    {
        get { return indexInInventory; }
        set { indexInInventory = value; }
    }

    /* Executes the specific Item's behaviour. */
    public abstract void itemOnClickBehavior();

    public abstract bool canStack(string otherItemName);
}