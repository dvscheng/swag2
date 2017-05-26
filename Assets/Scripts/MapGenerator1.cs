using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapGenerator1 : MonoBehaviour {
    int LENGTH = 21;
    int HEIGHT = 3;
    static int ITEMS_ARE_BLOCKS = 5;
    Sprite[] sprites;
    static string[] items = {"dirt", "gun"};
    HashSet<string> blockTypes;
    static string blockType = "Prefabs/Dirt";
    string path = "Prefabs/";
    static GameObject groundBlocks;

    public static int BULLET_LAYER;
    public static int UI_LAYER;
    public static int ITEM_LAYER;
    public static int PLAYER_LAYER;

    public Text blockTypeText;

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

        sprites = Resources.LoadAll<Sprite>("Sprites/Scavengers_SpriteSheet");
        makeObstacle();
    }

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

    public static void makeBlock(int x, int y)
    {
        GameObject go = (GameObject) Instantiate(Resources.Load(blockType), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
        go.name = blockType + " (" + x + ", " + y + ")";
        go.transform.parent = groundBlocks.transform;
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

        item.GameObject = (GameObject) Instantiate(Resources.Load("Prefabs/Items"), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
        item.Name = items[itemID];
        item.GameObject.GetComponent<ItemPrefabScript>().parentItem = item;
        SpriteRenderer sr = item.GameObject.GetComponent<SpriteRenderer>();
        sr.sprite = Resources.Load("Sprites/" + item.Name, typeof(Sprite)) as Sprite;
        return item;
    }

    public static void makeBullet(float x, float y, Vector2 velocity)
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet"), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
        // go.transform.parent = bullets.transform;
        go.layer = BULLET_LAYER;
        Rigidbody2D gorb = go.GetComponent<Rigidbody2D>();
        gorb.velocity = velocity;
        float angle = Vector2.Angle(Vector2.right, velocity);
        go.transform.Rotate(0, 0, angle);
    }
}
