using UnityEngine;
using Unity.Collections;

public class BulletManagerEnemy : MonoBehaviour
{

    public float velX = 5f;
    public float velY = 0f;
    Rigidbody2D rb;
    private Player player;
    private int Damage = 10;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObj = GameObject.Find("Player");
        player = playerObj.GetComponent<Player>();
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

        if (collision.name == "Player")
        {
            Destroy(gameObject);
            player.Damage(Damage);
        }
    }
}
