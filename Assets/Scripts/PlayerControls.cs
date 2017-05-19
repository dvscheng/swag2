using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    MapGenerator1 mg;
    Rigidbody2D rb;
    SpriteRenderer rendr;
    bool facingRight;
    bool canJump = true;
    int NUM_COLLIDERS = 4;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rendr = GetComponent<SpriteRenderer>();
        mg = FindObjectOfType(typeof(MapGenerator1)) as MapGenerator1;
    }

    private void Update()
    {
        /* Mouse checks. */
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayCoords = new Vector2(ray.origin.x, ray.origin.y);
            Collider2D collider = Physics2D.OverlapCircle(rayCoords, 0.01f);
            //RaycastHit hit;

            //if (Physics.Raycast(ray, out hit, 100))
            if (collider && collider.gameObject.name.CompareTo("Player") != 0)
            {
                //Object.Destroy(hit.transform.gameObject);
                Object.Destroy(collider.gameObject);
                //print("Collided");
            }
            else if (!collider)
            {
                mg.makeBlock((int) System.Math.Floor(rayCoords.x), (int) System.Math.Floor(rayCoords.y));
                //print("Missed");
            }
        }
    }

    private void FixedUpdate()
    {
        /* Controls (Physics). */
        Vector2 currVelocity = rb.velocity;
        bool movingLeft = Input.GetKey("left");
        bool movingRight = Input.GetKey("right");
        bool idle = !Input.GetKey("left") && !Input.GetKey("right");

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
            var groundObjects = GameObject.FindGameObjectsWithTag("ground");
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
}
