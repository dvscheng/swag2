using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    GameObject bullets;
    MapGenerator1 mg;
    Rigidbody2D rb;
    SpriteRenderer rendr;
    int NUM_COLLIDERS = 4;
    int MAX_INVENTORY_SIZE = 5;
    int itemIDInFocus;
    Item[] inventory;

    bool facingRight;
    int currentSize = 0;

    public Text debugModeText;
    bool debugMode = false;

    private void Start()
    {
        inventory = new Item[MAX_INVENTORY_SIZE];
        bullets = new GameObject();
        bullets.name = "Bullets";
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
            if (Input.GetKeyDown(KeyCode.Alpha0))
                itemID = 0;
            if (Input.GetKeyDown(KeyCode.Alpha1))
                itemID = 1;
            if (itemID >= 0)
            {
                Vector3 mousePos = Input.mousePosition;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Vector2 rayCoords = new Vector2(ray.origin.x, ray.origin.y);
                Collider2D collider = Physics2D.OverlapCircle(rayCoords, 0.01f);
                if (!collider)
                    MapGenerator1.generateItem((int)System.Math.Floor(rayCoords.x), (int)System.Math.Floor(rayCoords.y), itemID); //hardcoded 0
            } else
            {
                //error
            }
        }

        /* Mouse checks. */
        if (Input.GetMouseButtonDown(0))
        {
            //inventory[itemIDInFocus].itemOnClickBehavior();
        }
    }

    private void FixedUpdate()
    {
        /* Controls (Physics). */
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

    public int addToInventory(Item item)
    {
        print(item.Name);
        if (currentSize > MAX_INVENTORY_SIZE)
        {
            // error
            return -1;
        } else if (currentSize == MAX_INVENTORY_SIZE)
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
                    break;
                }
            }
            currentSize += 1;
            return 1;
        }
    }
}
