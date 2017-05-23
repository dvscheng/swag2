using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControls : MonoBehaviour
{
    MapGenerator1 mg;
    Rigidbody2D rb;
    SpriteRenderer rendr;
    bool facingRight;
    bool canJump = true;
    int NUM_COLLIDERS = 4;
    int BULLET_VELOCITY = 5;

    public Text shootModeText;

    bool shootMode = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rendr = GetComponent<SpriteRenderer>();
        mg = FindObjectOfType(typeof(MapGenerator1)) as MapGenerator1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            shootMode = !shootMode;
            shootModeText.text = "ShootMode: " + shootMode;
        }
        /* Mouse checks. */
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector2 rayCoords = new Vector2(ray.origin.x, ray.origin.y);
            if (shootMode)
            {
                bool rightSide = rayCoords.x - rb.position.x > 0;
                float offset = 0.5f;
                if (!rightSide)
                {
                    offset = -offset + -1;
                }
                makeBullet(rb.position.x + offset, rb.position.y, calculateNormalVector(rayCoords));
            } 
            else
            {
                Collider2D collider = Physics2D.OverlapCircle(rayCoords, 0.01f);
                //RaycastHit hit;

                if (collider && collider.CompareTag("block"))
                {
                    //Object.Destroy(hit.transform.gameObject);
                    Object.Destroy(collider.gameObject);
                    //print("Collided");
                }
                else if (!collider)
                {
                    mg.makeBlock((int)System.Math.Floor(rayCoords.x), (int)System.Math.Floor(rayCoords.y));
                    //print("Missed");
                }
            }
            
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

    private Vector2 calculateNormalVector(Vector2 targetPosition)
    {
        Vector2 heading = targetPosition - rb.position;
        var distance = heading.magnitude;
        return heading / distance * BULLET_VELOCITY;
    }

    private void makeBullet(float x, float y, Vector2 velocity) 
    {
        GameObject go = (GameObject)Instantiate(Resources.Load("Prefabs/Bullet"), new Vector3(x + 0.5f, y + 0.5f, 0), Quaternion.identity);
        Rigidbody2D gorb = go.GetComponent<Rigidbody2D>();
        gorb.velocity = velocity;
        float angle = Vector2.Angle(Vector2.right, velocity);
        go.transform.Rotate(0, 0, angle);
    }
}
