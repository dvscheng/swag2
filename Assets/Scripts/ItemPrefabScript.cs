using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPrefabScript : MonoBehaviour {
    /* Components. */
    public BoxCollider2D bc1;
    public BoxCollider2D bc2;
    public CircleCollider2D pbc;

    /* Misc. */
    public Item parentItem;

    public Item ParentItem
    {
        get { return parentItem; }
        set { parentItem = value; }
    }

    void Start() {
        pbc = GameObject.Find("Player").GetComponent<CircleCollider2D>();
        Physics2D.IgnoreLayerCollision(MapGenerator1.UI_LAYER, MapGenerator1.ITEM_LAYER, true);
        Physics2D.IgnoreCollision(bc1, pbc, true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Player"))
        {
            /* If item is successfully added to inventory, then destroy its GameObject. */
            PlayerControls pc = collision.gameObject.GetComponent<PlayerControls>();
            if (pc.addToInventory(parentItem) == 1)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}
