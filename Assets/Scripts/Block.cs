using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : Item {
    /* Creates this block (or destroys a block, for now) at the mouse's position. */
    override
    public void itemOnClickBehavior()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 rayCoords = new Vector2(ray.origin.x, ray.origin.y);
        Collider2D collider = Physics2D.OverlapCircle(rayCoords, 0.01f);

        if (collider && collider.CompareTag("block"))
        {
            //Object.Destroy(collider.gameObject);
        }
        else if (!collider)
        {
            MapGenerator1.makeBlock((int) System.Math.Floor(rayCoords.x), (int) System.Math.Floor(rayCoords.y), name);
            amount--;
            GameObject.Find("Player").GetComponent<PlayerControls>().updateHotKeyItemCount(indexInInventory);
            if (amount == 0)
            {
                GameObject.Find("Player").GetComponent<PlayerControls>().deleteItem(indexInInventory);
            }
        }
    }

    override
    public bool canStack(string otherItemName)
    {
        return name.Equals(otherItemName);
    }
}
