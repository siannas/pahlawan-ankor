using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EnemiesBehavior : MonoBehaviour
{
    private Player playerevent;
    private SmoothCam playercam;
    [HideInInspector]
    public bool IsTweening;
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
        IsTweening = false;
        playerevent = GameObject.Find("Player").GetComponent<Player>();
        playercam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCam>();
        enemies = GameObject.FindGameObjectWithTag("Enemy");
        curHealth = maxHealth;
        healthtxt = GameObject.Find("HealthText").GetComponent<TextMeshProUGUI>();
        renderer = GetComponent<SpriteRenderer>();
        patrol = GetComponent<EnemiesPatrol>();
        StartCoroutine(Firing());
        StartCoroutine(Jumpingenemy());
        
        if (playerevent == null)
        {
            Debug.Log("Null");
        }
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
            if (GameObject.Find("Boss") != null)
            {
                playercam.SetZoom(0.75f);

                if (IsTweening == false)
                {
                    playerevent.Bossdead();
                    IsTweening = true;
                }
            }
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
        StartCoroutine(Jumpingenemy());
    }
}
