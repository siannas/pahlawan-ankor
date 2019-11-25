using UnityEngine;

public class EnemiesPatrol : MonoBehaviour
{
    [HideInInspector]
    public bool facingRight = true;
    public float jumpHeight = 0f;
    public Vector2 speed;
    public Vector2 direction = new Vector2(1, 0);
    public Transform Collidercheck;
    public Transform enemies;
    private float distance = 1f;

    [System.Obsolete]
    private void Start()
    {
        enemies = transform.parent;
        Collidercheck = transform.FindChild("ColliderCheck").GetComponentInChildren<Transform>();
    }

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

        RaycastHit2D groundInfo = Physics2D.Raycast(Collidercheck.position, Vector2.down, distance);
        //Debug.DrawLine(Collidercheck.transform.position, Collidercheck.transform.forward, Color.white, 5f, false);
        //Debug.Log(groundInfo);
        if (groundInfo.collider == false)
        {
            if (!facingRight)
            {
                direction = Vector2.Scale(direction, new Vector2(1, 0));
                Flip();
            }
            else if (facingRight)
            {
                direction = Vector2.Scale(direction, new Vector2(-1, 0));
                Flip();
            }
        }
        //Debug.Log(speed);
    }


    private void Flip()
    {
        facingRight = !facingRight;
        if (!facingRight)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            //Collidercheck.transform.position = new Vector3(this.transform.position.x - 0.5f, this.transform.position.y - 1f, 0);
        }
        else if (facingRight)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            //Collidercheck.transform.position = new Vector3(this.transform.position.x + 0.5f, this.transform.position.y - 1f, 0);
        }
    }

    public void Jumping()
    {
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, jumpHeight), ForceMode2D.Impulse);
    }

}
