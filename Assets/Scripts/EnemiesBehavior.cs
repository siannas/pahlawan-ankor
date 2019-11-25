using System.Collections;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class EnemiesBehavior : MonoBehaviour
{
  
    private EnemiesPatrol patrol;
    private SpriteRenderer renderer;
    public int curHealth;
    public int maxHealth = 150;
    public TextMeshProUGUI healthtxt;
    public GameObject enemies;
    private float RandomJump;
    private float RandomFire;
    public GameObject BulletLeftEnemy, BulletRightEnemy, Medkit;
    Vector2 bulletPos;
    private Pause pause;
    bool medkitcreated = false;


    // Start is called before the first frame update
    void Start()
    {
        pause = GameObject.Find("Main Camera").GetComponent<Pause>();
        enemies = this.gameObject;
        curHealth = maxHealth;
        healthtxt = GetComponentInChildren<TextMeshProUGUI>();
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
        if (curHealth <= 0)
        {
            Destroy(gameObject);
            SoundsManager.PlaySound("death");
            pause.enemiescount -= 1f;
            Debug.Log(pause.enemiescount);
        }

        if (gameObject.name == "Boss")
        {
            if (curHealth <= 50)
            {
                renderer.color = new Color(1f, 0.5f, 0.5f, 1f);
                patrol.speed = new Vector2(5, 0);
            }
        }
    }

    IEnumerator Firing()
    {
        {

            if (gameObject.name == "Boss")
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

                    if (medkitcreated == false)
                    {
                        Instantiate(Medkit, gameObject.transform.position, Quaternion.identity);
                        Debug.Log("Medkit created");
                        medkitcreated = true;
                    }

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

            RandomFire = Random.Range(1f, 2f);
            if (gameObject.name == "Mob")
            {

                if (curHealth != 0)
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

                    yield return new WaitForSeconds(RandomFire);
                }

                StartCoroutine(Firing());
            }
        }
    }

    IEnumerator Jumpingenemy()
    {
        RandomJump = Random.Range(1f, 4f);
        yield return new WaitForSeconds(RandomJump);
        {
            patrol.Jumping();
        }
        StartCoroutine(Jumpingenemy());
    }
}
