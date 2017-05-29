using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    /* Misc. */
    Vector2 origin;
    float MAX_DISTANCE = 15;

    private void Start()
    {
        /* Ignores the collision between bullets and the UI. */
        Physics2D.IgnoreLayerCollision(MapGenerator1.UI_LAYER, MapGenerator1.BULLET_LAYER, true);

        origin = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Object.Destroy(gameObject);
    }

    private void Update()
    {
        /* If the bullet is further than MAX_DISTANCE away from its origin, destroy it. */
        if (System.Math.Abs(transform.position.x - origin.x) > MAX_DISTANCE
            || System.Math.Abs(transform.position.y - origin.y) > MAX_DISTANCE)
        {
            Object.Destroy(gameObject);
        }
    }
}
