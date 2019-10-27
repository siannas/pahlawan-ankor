using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{

    public float velX = 5f;
    float velY = 0f;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.CompareTag("Solid") || collision.CompareTag("Platform"))
        {
            Destroy(gameObject);
        }
    }
}
