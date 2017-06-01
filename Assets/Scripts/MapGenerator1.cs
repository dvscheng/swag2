using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator1 : MonoBehaviour {
    /* Groupings. */
    static GameObject groundBlocks;
    static GameObject playerMadeBlocks;
    static GameObject droppedItems;

    /* Layers. */
    public static int BULLET_LAYER;
    public static int UI_LAYER;
    public static int ITEM_LAYER;
    public static int PLAYER_LAYER;

    /* Text, Unity-related. */
    public Text blockTypeText;

    /* Dimensions for the arena. */
    static int LENGTH = 21;
    static int HEIGHT = 3;

    /* Item system misc. */
    static int ITEMS_ARE_BLOCKS = 3;
    static string[] items = {"dirt", "vines", "mushroom", "gun", "sword"};

    // may be obsolete or needs changing
    HashSet<string> blockTypes;
    static string blockType = "Prefabs/Dirt";
    string path = "Prefabs/";
    Sprite[] sprites;

    private void Start() 
    {
        BULLET_LAYER = LayerMask.NameToLayer("Bullet");
        ITEM_LAYER = LayerMask.NameToLayer("Item");
        UI_LAYER = LayerMask.NameToLayer("UI");
        PLAYER_LAYER = LayerMask.NameToLayer("Player");

        blockTypes = new HashSet<string>();
        blockTypes.Add("Dirt");
        blockTypes.Add("Vines");
        blockTypes.Add("Person");

        groundBlocks = new GameObject();
        groundBlocks.name = "Ground blocks";

        playerMadeBlocks = new GameObject();
        playerMadeBlocks.name = "Player-made blocks";

        droppedItems = new GameObject();
        droppedItems.name = "Dropped items";

        sprites = Resources.LoadAll<Sprite>("Sprites/Scavengers_SpriteSheet");
        makeObstacle();
    }

    /* Sets the type of the block to be set. (change)*/
    public void setBlockType(string name)
    {
        if (!blockTypes.Contains(name))
        {
            Debug.Log("Invalid name passed into setBlockType(): " + name);
            name = path + "Dirt";
        }
        blockTypeText.text = "BlockType: " + name;
        blockType = path + name;
    }
    
    /* Makes a regular rectangular field of dimensions LENGTH by HEIGHT. */
    private void makeRegularField()
    {
        for (int x = 0; x < LENGTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                makeBlock(-LENGTH / 2 + x, -HEIGHT + y);
            }
        }
    }

    /* Makes a regular rectangular field of dimensions LENGTH by HEIGHT and
     * randomly adds blocks on the top layer as obstacles. */
    private void makeObstacle()
    {
        for (int x = 0; x < LENGTH; x++)
        {
            for (int y = 0; y < HEIGHT; y++)
            {
                makeBlock(-LENGTH / 2 + x, -HEIGHT + y);
                if ((y + 1 == HEIGHT) && (x % Random.Range(1, 5) == 0) && (x != LENGTH/2))
                {
                    makeBlock(-LENGTH / 2 + x, -HEIGHT + y+1);
                }
            }
        }
    }

    /* Used under-the-hood to make terrain. */
    public static void makeBlock(int x, int y)
    {
        Object obj = Resources.Load(blockType); // defaulted to using dirt prefab
        if (obj)
        {
            GameObject go = (GameObject)Instantiate(obj, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
            go.name = blockType + " (" + x + ", " + y + ")";
            go.transform.parent = groundBlocks.transform;
        }
        /*
        GameObject ob = new GameObject();
        SpriteRenderer renderer = ob.AddComponent<SpriteRenderer>();
        Vector3 pos = new Vector3(x + 0.5f, y + 0.5f);

        renderer.sprite = sprites[32];
        renderer.name = "Dirt (" + x + ", " + y + ")";
        //renderer.sprite = Resources.Load("Sprites/Scavengers_SpriteSheet", typeof(Sprite)) as Sprite;
        ob.transform.position += pos;

        BoxCollider2D boxCollider = ob.AddComponent<BoxCollider2D>();
        boxCollider.sharedMaterial = Resources.Load("Materials/Frictionless", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
        ob.tag = "ground";
        */
    }

    /* Used by the player to generate blocks. */
    public static void makeBlock(int x, int y, string blockName)
    {
        print(blockName);
        if (itemIsBlock(blockName))
        {
            Object obj = Resources.Load(blockType); // defaulted to using dirt prefab
            if (obj)
            {
                GameObject go = (GameObject) Instantiate(obj, new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
                go.name = blockName + " (" + x + ", " + y + ")";
                go.transform.parent = playerMadeBlocks.transform;

                if (blockName.Equals("mushroom"))
                {
                    go.GetComponent<BoxCollider2D>().sharedMaterial = Resources.Load("Materials/MushroomBlockMaterial", typeof(PhysicsMaterial2D)) as PhysicsMaterial2D;
                }
                // Either use Sprite.Create() or make separate sprite with 32 ppu
                SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
                sr.sprite = Resources.Load("Sprites/" + blockName, typeof(Sprite)) as Sprite;
            }
        }
    }

    /* Returns true if the block name is in the items array AND is of block type. */
    private static bool itemIsBlock(string blockName)
    {
        for (int i = 0; i < ITEMS_ARE_BLOCKS; i++)
        {
            if (items[i].Equals(blockName)) {
                return true;
            }
        }
        return false;
    }

    /*
     * Generate GameObject in Unity given position and the Item to generate
     * */
    public static Item drawItem(int x, int y, Item item)
    {
        GameObject gameObj = (GameObject)Instantiate(Resources.Load("Prefabs/Items"), new Vector3(x, y, 0), Quaternion.identity);
        gameObj.transform.localScale = new Vector3(0.5f, 0.5f, 1);
        gameObj.name = item.Name;
        gameObj.GetComponent<ItemPrefabScript>().parentItem = item;
        gameObj.transform.parent = droppedItems.transform;
        SpriteRenderer sr = gameObj.GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load("Sprites/" + item.Name, typeof(Sprite)) as Sprite;
        item.GameObject = gameObj;
        return item;
    }

    /* Generates and returns an item given coordinates and an ID. */
    public static Item generateItem(int x, int y, int itemID)
    {
        Item item;
        if (itemID < ITEMS_ARE_BLOCKS)
        {
            item = new Block();
        }
        else
        {
            item = new Weapon();
        }
        item.Name = items[itemID];

        return drawItem(x, y, item);
    }

    /* Drops the item from the player. facingRight is a player variable. */
    public static void dropItem(float x, float y, bool facingRight, Item item)
    {
        if (facingRight)
        {
            x = x + 2;
        } else
        {
            x = x - 2;
        }

        drawItem((int) x, (int) y, item);
    }

    /* Shoots a bullet. */
    public static void makeBullet(float x, float y, Vector2 velocity)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet"), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
        go.name = "Bullet Pos: (" + x + ", " + y + ") Velocity: (" + velocity.x + ", " + velocity.y + ")";
        go.transform.parent = GameObject.Find("Bullets").transform;
        go.layer = BULLET_LAYER;
        Rigidbody2D gorb = go.GetComponent<Rigidbody2D>();
        gorb.velocity = velocity;
        float sign = (velocity.y < 0) ? -1.0f : 1.0f;
        float angle = Vector2.Angle(Vector2.right, velocity) * sign;
        go.transform.Rotate(0, 0, angle);
    }
}
