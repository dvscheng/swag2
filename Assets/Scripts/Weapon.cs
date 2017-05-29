using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Item {
    private int damage;
    private int weaponType = 0; // 0 = ranged, 1 = melee
    private int BULLET_VELOCITY = 10;

    /* Shoots a projectile or melees depending on weaponType. */
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

    /* Shoots a bullet at mouse position. */
    private void rangedHit(Collider2D collider, Vector2 rayCoords)
    {
        bool hitUI = collider && collider.gameObject.layer == 5;

        if (!hitUI)
        {
            GameObject player = GameObject.Find("Player");
            bool rightSide = rayCoords.x - player.transform.position.x > 0;
            float offset = 0.5f;
            if (!rightSide)
            {
                offset = -offset + -1;
            }
            MapGenerator1.makeBullet(player.transform.position.x + offset, player.transform.position.y, calculateNormalVector(rayCoords));
        }
    }

    private void meleeHit()
    {
        //swag
    }

    /* Swag*/
    private Vector2 calculateNormalVector(Vector2 targetPosition)
    {
        GameObject player = GameObject.Find("Player");
        Vector2 gameObPos = player.transform.position;
        Vector2 heading = targetPosition - gameObPos;
        var distance = heading.magnitude;
        return heading / distance * BULLET_VELOCITY;
    }
}
