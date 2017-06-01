using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSlashScript : MonoBehaviour {
    int maxFrames = 60;
    int frames = 0;
	// Use this for initialization
	void Start () {
        frames = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (frames >= maxFrames)
        {
            print("reached 60 frames");
            Destroy(gameObject);
        }
        else
        {
            frames++;
        }
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("enter trigger");
        if (collision.GetComponent<Collider2D>().CompareTag("block"))
        {
            Destroy(collision.GetComponent<Collider2D>().gameObject);
        }
        else if (collision.GetComponent<Collider2D>().CompareTag("enemy"))
        {
            // do things if it attacks other players rather than blocks
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("enter collision");
        if (collision.collider.CompareTag("block"))
        {
            Destroy(collision.collider.gameObject);
        } else if (collision.collider.CompareTag("enemy"))
        {
            // do things if it attacks other players rather than blocks
        }
        Destroy(gameObject);
    }
}
