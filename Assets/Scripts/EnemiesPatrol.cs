using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesPatrol : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;
    public float jumpHeight = 0f;
    public Vector2 speed;
    public Vector2 direction = new Vector2(1, 0);

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Wall") || collision.collider.CompareTag("Player") || collision.collider.CompareTag("Spikes"))
        {
            direction = Vector2.Scale(direction, new Vector2(-1, 0));
        }

        if (!collision.collider.CompareTag("Solid"))
        {
            if (!facingRight)
            {
                Flip();
            }
            else if (facingRight)
            {
                Flip();
            }
        }
    }


    void FixedUpdate()
    {
        Vector2 movement = new Vector2(speed.x * direction.x, 0);
        movement *= Time.deltaTime;
        transform.Translate(movement);
        //Debug.Log(speed);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        if (!facingRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (facingRight)
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    public void Jumping()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

}
