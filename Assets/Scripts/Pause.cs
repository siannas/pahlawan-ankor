using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject PauseUI;
    private DialogueManager Dialogmanager;
    private bool dialogcomplete;
    private readonly string main;
    private GameObject[] enemies;
    private Player playerevent;
    private SmoothCam playercam;
    private SpriteRenderer FaderImg;
    [HideInInspector] public bool IsTweening;
    [HideInInspector] public float enemiescount;

    public static bool paused = false;

    void Start()
    {
        paused = false;
        FaderImg = GameObject.Find("Fader").GetComponent<SpriteRenderer>();
        IsTweening = false;
        playerevent = GameObject.Find("Player").GetComponent<Player>();
        playercam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<SmoothCam>();

        PauseUI.SetActive(false);
        if (main == SceneManager.GetActiveScene().name)
        {
            Dialogmanager = GameObject.Find("dialogueMaster").GetComponent<DialogueManager>();
        }

        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        
        enemiescount = enemies.Length;
        Debug.Log(enemiescount);
        StartCoroutine(Fader(true));
    }

    void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            paused = !paused;
        }

        if (paused)
        {
            PauseUI.SetActive(true);
            Time.timeScale = 0;
        }

        if (!paused)
        {
            PauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }


    private void FixedUpdate()
    {
        if (main == SceneManager.GetActiveScene().name)
        {
            dialogcomplete = Dialogmanager.dialoguecomplete;
            if (dialogcomplete == true)
            {
                dialogcomplete = false;
                //Debug.Log("dialogcomplete");
                paused = false;
            }
        }
        

        if (enemiescount == 0)
        {
            playercam.SetZoom(0.75f);

            if (IsTweening == false)
            {
                playerevent.AllDead();
                IsTweening = true;
                StartCoroutine(Fader(false));
            }
        }
    }
    public void Resume()
    {
        paused = false;
    }

    public void Restart ()
    {
        paused = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit ()
    {
        SceneManager.LoadScene(0);
    }

    IEnumerator Fader(bool Fading)
    {
        
        if (Fading)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                FaderImg.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            yield return new WaitForSeconds(2f);
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                FaderImg.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }

    }
}
