using UnityEngine;

public class BulletManager : MonoBehaviour
{

    [HideInInspector]
    public float velX = 5f;
    [HideInInspector]
    public float velY = 0f;
    Rigidbody2D rb;
    private EnemiesBehavior enemybehavior;
    private GameObject[] enemies;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        for (int i = 0; i < enemies.Length; i++)
        {
            enemybehavior = enemies[i].GetComponent<EnemiesBehavior>();
        }
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

        if (collision.CompareTag("Enemy"))
        {
            Destroy(gameObject);
            enemybehavior = collision.GetComponent<EnemiesBehavior>();
            enemybehavior.curHealth -= 10;
        }
    }
}
