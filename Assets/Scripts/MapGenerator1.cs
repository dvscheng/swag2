using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator1 : MonoBehaviour {
    int LENGTH = 21;
    int HEIGHT = 3;
    Sprite[] sprites;

    private void Start()
    {
        sprites = Resources.LoadAll<Sprite>("Sprites/Scavengers_SpriteSheet");
        makeObstacle();
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

    public void makeBlock(int x, int y)
    {
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
    }
}
