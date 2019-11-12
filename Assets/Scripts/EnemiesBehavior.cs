using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EnemiesBehavior : MonoBehaviour
{
    private SmoothCam playercam;

    private EnemiesPatrol patrol;
    SpriteRenderer renderer;
    public int curHealth;
    public int maxHealth = 150;
    public TextMeshProUGUI healthtxt;
    public GameObject enemies;
    private float Rand;

    public GameObject BulletLeftEnemy, BulletRightEnemy;
    Vector2 bulletPos;

    // Start is called before the first frame update
    void Start()
    {
        playercam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCam>();
        enemies = GameObject.FindGameObjectWithTag("Enemy");
        curHealth = maxHealth;
        healthtxt = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        renderer = GetComponent<SpriteRenderer>();
        patrol = GetComponent<EnemiesPatrol>();
        StartCoroutine(Firing());
        StartCoroutine(Jumpingenemy());
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
            playercam.SetZoom(0.75f);
        }

        if (curHealth <= 50)
        {
            renderer.color = new Color(1f, 0.5f, 0.5f, 1f);
            patrol.speed = new Vector2(5, 0);
        }
    }

    IEnumerator Firing()
    {
        {

            if (curHealth >= 50)
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
            }

            if (curHealth <= 50)
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

                yield return new WaitForSeconds(0.5f);
            }

            StartCoroutine(Firing());
        }
    }

    IEnumerator Jumpingenemy()
    {
        Rand = Random.Range(1f, 4f);
        yield return new WaitForSeconds(Rand);
        {
            patrol.Jumping();
        }
        //Debug.Log(Rand);
        StartCoroutine(Jumpingenemy());
    }
}
