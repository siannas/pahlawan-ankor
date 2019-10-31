﻿using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [HideInInspector]
    public float velX = 5f;
    [HideInInspector]
    public float velY = 0f;
    Rigidbody2D rb;
    private EnemiesBehavior enemybehavior;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject enemies = GameObject.Find("Boss");
        enemybehavior = enemies.GetComponent<EnemiesBehavior>();
    }

    void Update()
    {
        rb.velocity = new Vector2(velX, velY);
        Destroy(gameObject, 2f);
    }

    void OnTriggerEnter2D(Collider2D collision)

    {
        if (collision.CompareTag("Solid") || collision.CompareTag("Platform") || collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }

        if (collision.name == "Boss")
        {
            Destroy(gameObject);
            enemybehavior.curHealth -= 10;
        }
    }
}
