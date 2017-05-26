using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
    private int damage;
    private int weaponType; // 0 = ranged, 1 = melee
    private int BULLET_VELOCITY = 10;

    override
    public void itemOnClickBehavior()
    {
        Vector3 mousePos = Input.mousePosition;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector2 rayCoords = new Vector2(ray.origin.x, ray.origin.y);
        Collider2D collider = Physics2D.OverlapCircle(rayCoords, 0.01f);

        if (weaponType == 0)
            rangedHit(collider, rayCoords);
        else
            meleeHit();
    }


    private void rangedHit(Collider2D collider, Vector2 rayCoords)
    {
        bool hitUI = collider && collider.gameObject.layer == 5;

        if (!hitUI)
        {
            bool rightSide = rayCoords.x - gameObject.transform.position.x > 0;
            float offset = 0.5f;
            if (!rightSide)
            {
                offset = -offset + -1;
            }
            MapGenerator1.makeBullet(gameObject.transform.position.x + offset, gameObject.transform.position.y, calculateNormalVector(rayCoords));
        }
    }

    private void meleeHit()
    {
        //swag
    }

    private Vector2 calculateNormalVector(Vector2 targetPosition)
    {
        Vector2 gameObPos = gameObject.transform.position;
        Vector2 heading = targetPosition - gameObPos;
        var distance = heading.magnitude;
        return heading / distance * BULLET_VELOCITY;
    }
}
