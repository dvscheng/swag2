using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    /* Components. */
    MapGenerator1 mg;
    Rigidbody2D rb;
    SpriteRenderer rendr;

    /* Final parameters. */
    int NUM_COLLIDERS = 4;
    int MAX_INVENTORY_SIZE = 5;
    int NUM_HOTKEYS = 5;

    /* Inventory management variables. */
    int hotkeyIDInFocus;
    int inventoryCurrentSize = 0;
    Item[] inventory;
    GameObject[] hotkeys;

    /* Text, Unity-related. */
    public Text debugModeText;
    public Text currentItemText;

    /* Misc. */
    bool facingRight;
    bool debugMode = false;
    GameObject bullets;

    private void Start()
    {
        /* Initialize inventory and hotkeys arrays. */
        inventory = new Item[MAX_INVENTORY_SIZE];
        hotkeyIDInFocus = -1;   // defaults to no hotkey in focus (-1).
        hotkeys = new GameObject[NUM_HOTKEYS];
        for (int i = 0; i < NUM_HOTKEYS; i++)
        {
            hotkeys[i] = GameObject.Find("Hotkey " + (i + 1));
        }

        /* Initialize misc. */
        bullets = new GameObject();
        bullets.name = "Bullets";

        /* Get components. */
        rb = GetComponent<Rigidbody2D>();
        rendr = GetComponent<SpriteRenderer>();
        mg = FindObjectOfType(typeof(MapGenerator1)) as MapGenerator1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            debugMode = !debugMode;
            debugModeText.text = "DebugMode: " + debugMode;
        }

        if (debugMode)
        {
            int itemID = -1;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                itemID = 0;
            if (Input.GetKeyDown(KeyCode.Alpha2))
                itemID = 1;
            if (Input.GetKeyDown(KeyCode.Alpha3))
                itemID = 2;
            if (Input.GetKeyDown(KeyCode.Alpha4))
                itemID = 3;
            if (Input.GetKeyDown(KeyCode.Alpha5))
                itemID = 4;
            if (itemID >= 0)
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector2 rayCoords = new Vector2(ray.origin.x, ray.origin.y);
                Collider2D collider = Physics2D.OverlapCircle(rayCoords, 0.01f);
                if (!collider)
                    MapGenerator1.generateItem((int)System.Math.Floor(rayCoords.x), (int)System.Math.Floor(rayCoords.y), itemID);
            } else
            {
                //display: choose an item
            }
        }

        /* Inputs (eventually separate for custom keybinds). */
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            hotkeyIDInFocus = 0;
            currentItemText.text = "Selected 1";
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            hotkeyIDInFocus = 1;
            currentItemText.text = "Selected 2";
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            hotkeyIDInFocus = 2;
            currentItemText.text = "Selected 3";
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            hotkeyIDInFocus = 3;
            currentItemText.text = "Selected 4";
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            hotkeyIDInFocus = 4;
            currentItemText.text = "Selected 5";
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            dropSelectedItem();
        }
        if (Input.GetMouseButtonDown(0))
        {
            /* If there is a hotkey in focus AND an item in the hotkey, then run the item's behavior. */
            if (hotkeyIDInFocus >= 0)
            {
                Item item = inventory[hotkeyIDInFocus];
                if (item != null)
                {
                    item.itemOnClickBehavior();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        /* Controls moving and jumping (Physics). */
        Vector2 currVelocity = rb.velocity;
        bool movingLeft = Input.GetKey(KeyCode.A);
        bool movingRight = Input.GetKey(KeyCode.D);
        bool idle = !movingLeft && !movingRight;

        if (movingLeft)
        {
            facingRight = false;
            rb.velocity = new Vector2(-5, currVelocity.y);
        } else if (movingRight)
        {
            facingRight = true;
            rb.velocity = new Vector2(5, currVelocity.y);
        } else if (idle)
        {
            rb.velocity = new Vector2(0, currVelocity.y);
        }

        if (Input.GetKeyDown("space"))
        {
            // maybe look in the vicinity of the character instead
            var groundObjects = GameObject.FindGameObjectsWithTag("block");
            foreach (var ground in groundObjects)
            {
                var groundCollider = ground.GetComponent<BoxCollider2D>();
                if (rb.IsTouching(groundCollider))
                {
                    rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
                    break;
                }
            }
        }

        rendr.flipX = facingRight;
    }

    /* Adds an item to the player's inventory if the inventory is not full.
     * Returns -1 if the current inventory size is somehow greater than the max size 
     *  or if an error has occured when adding the item
     * Returns 0 if the inventory is full.
     * Returns 1 if successfully added the item to inventory. */
    public int addToInventory(Item item)
    {
        if (inventoryCurrentSize > MAX_INVENTORY_SIZE)
        {
            // error
            return -1;
        } else if (inventoryCurrentSize == MAX_INVENTORY_SIZE)
        {
            // display inventory full
            return 0;
        } else
        {
            for (int i = 0; i < MAX_INVENTORY_SIZE; i++)
            {
                if (inventory[i] == null)
                {
                    inventory[i] = item;
                    displayOnHotkeyBar(i);
                    inventoryCurrentSize += 1;
                    return 1;
                }
            }
            return -1;
        }
    }

    /* After checking that there is a hotkey in focus and an item in that slot,
     *  1. Generates the item in-game. 
     *  2. Effectively remove the item from inventory. 
     *  3. Display a blank sprite on the hotkey at the index in focus. 
     *  4. Decrease the current size of the inventory. */
    public void dropSelectedItem()
    {
        if (hotkeyIDInFocus != -1)
        {
            Item item = inventory[hotkeyIDInFocus];
            if (item != null)
            {
                MapGenerator1.dropItem(gameObject.transform.position.x, gameObject.transform.position.y, facingRight, item);
                inventory[hotkeyIDInFocus] = null;
                displayOnHotkeyBar(hotkeyIDInFocus);
                inventoryCurrentSize -= 1;
            }
        }
    }

    /* If the item DOESN'T exist (was dropped or something), set the color of the 
     *  hotkeyBar slot to grey-ish.
     * If the item DOES exist, set the color of the slot to white so that the newly
     *  loaded sprite is not grey-ish. */
    public void displayOnHotkeyBar(int index)
    {
        Item item = inventory[index];
        Image hotkeyImage = hotkeys[index].GetComponent<Button>().image;
        if (item == null)
        {
            hotkeyImage.color = new Color(0xA3, 0xA3, 0xA3, 0xFF);  // grey-ish = A3A3A3FF
            hotkeyImage.sprite = null;
        } else
        {
            hotkeyImage.color = Color.white;
            hotkeyImage.sprite = Resources.Load("Sprites/" + item.Name, typeof(Sprite)) as Sprite;
        }
    }
}
