using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemiesBehavior : MonoBehaviour
{
    private EnemiesPatrol patrol;
    
    public int curHealth;
    public int maxHealth = 100;
    public TextMeshProUGUI healthtxt;
    public GameObject enemies;

    public GameObject BulletLeftEnemy, BulletRightEnemy;
    Vector2 bulletPos;

    // Start is called before the first frame update
    void Start()
    {
        enemies = GameObject.FindGameObjectWithTag("Enemy");
        curHealth = maxHealth;
        healthtxt = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        //patrol.GetComponent<SpriteRenderer>();
        StartCoroutine(Firing());
    }

    // Update is called once per frame
    void Update()
    {
        healthtxt.text = curHealth.ToString() + "%";
    }

    private void FixedUpdate()
    {
        if (curHealth == 0)
        {
            Destroy(gameObject);
        }
    }

    IEnumerator Firing ()
    {
        {
    
            bulletPos = transform.position;
            if (GetComponent<SpriteRenderer>().flipX == false)
            {
                bulletPos += new Vector2(0, -0.1f);
                Instantiate(BulletRightEnemy, bulletPos, Quaternion.identity);
            }
            else
            {
                bulletPos += new Vector2(0, -0.1f);
                Instantiate(BulletLeftEnemy, bulletPos, Quaternion.identity);
            }
            yield return new WaitForSeconds(1f);

            StartCoroutine(Firing());

        }
    }
}
