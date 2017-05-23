using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour {

    float MAX_DISTANCE = 15;
    Vector2 origin;

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(MapGenerator1.UI_LAYER, MapGenerator1.BULLET_LAYER, true);
        origin = new Vector2(transform.position.x, transform.position.y);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //print("layer: " + collision.gameObject.layer + " other collider layer: " + collision.collider.gameObject.layer);
        if (collision.collider.gameObject.layer == MapGenerator1.UI_LAYER)
        {
            //print("swag out in ignorecolision");
            Physics2D.IgnoreCollision(collision.collider, collision.otherCollider, true);
        } else
        {
            //print("collided swagbullet");
            Object.Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (System.Math.Abs(transform.position.x - origin.x) > MAX_DISTANCE || System.Math.Abs(transform.position.y - origin.y) > MAX_DISTANCE)
        {
            Object.Destroy(gameObject);
        }
    }
}
