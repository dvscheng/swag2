using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item {
    protected GameObject gameObject;
    protected string name;
    protected int amount;

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
            gameObject.name = value;
        }
    }

    public int Amount
    {
        get { return amount; }
        set { amount = value; }
    }

    public abstract void itemOnClickBehavior();
}