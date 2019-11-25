﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using KoganeUnityLib;
using System.Text.RegularExpressions;

public class DialogueManager : MonoBehaviour
{
    public GameObject LEFT;
    public GameObject RIGHT;
    public GameObject dialogBox;
    
    GameObject dialogMaster;
    TextMeshProUGUI dialogLine;

    public TMP_Typewriter m_typewriter;
    public float m_speed;
    private bool complete = true;
    [HideInInspector]
    public bool dialoguecomplete;
    private bool player;
    private bool enemy;
    private GameObject[] enemies;
    private EnemiesBehavior[] EnemiesBehavior;
    private EnemiesPatrol[] EnemiesPatrols;

    //private Pause pause;

    private dialogueTemplate dt;

    private Dictionary<string, Sprite> person_image;

    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>().enabled = false;
        player = GameObject.Find("Player").GetComponent<MovementController>().enabled = false;
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        EnemiesBehavior = new EnemiesBehavior[enemies.Length];
        EnemiesPatrols = new EnemiesPatrol[enemies.Length];
        Debug.Log(enemies.Length);
        for (int i = 0; i < enemies.Length; i++)
        {
           enemies[i].GetComponent<EnemiesBehavior>().enabled = false;
           enemies[i].GetComponent<EnemiesPatrol>().enabled = false;
        }
        dialogMaster = gameObject;
        dialogLine = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        dt = gameObject.transform.GetComponentInChildren<dialogueTemplate>();
        person_image = new Dictionary<string, Sprite>();

        initScenario(0);
    }

    private void initScenario(int index)
    {
        dt.startScenarioAt(index);

        //init image sprites
        Dictionary<string, string> spritesPath = dt.getSpritesPath();
        
        foreach(KeyValuePair<string,string> path in spritesPath)
        {
            Sprite image = Resources.Load<Sprite>(path.Value);
            Debug.Log(path.Value);
            person_image.Add(path.Key, image);
        }
        

        showHide("dialog", true);
        continueLine();
    }

    private void showDialogLine(string line)
    {
        complete = false;
        m_typewriter.Play
        (
            text: line,
            speed: m_speed,
            onComplete: () => complete = true
        );
    }

    public void continueLine()
    {
        if (!complete){
            m_typewriter.Skip();
        }
        else
        {

            string line = dt.getNextLine();

            if (string.IsNullOrEmpty(line) && dialoguecomplete == false)
            {
                showHide("dialog", false);
                //Debug.Log("DONE");
                dialoguecomplete = true;
                player = GameObject.Find("Player").GetComponent<Player>().enabled = true;
                player = GameObject.Find("Player").GetComponent<MovementController>().enabled = true;
                for (int i = 0; i < enemies.Length; i++)
                {
                    enemies[i].GetComponent<EnemiesBehavior>().enabled = true;
                    enemies[i].GetComponent<EnemiesPatrol>().enabled = true;
                }
                return;
            }

            changePerson();
            showDialogLine(line);
        }
    }

    private void changePerson()
    {
        string person_key = dt.getKey();

        switch (dt.getCurrPersonPosition())
        {
            case "LEFT":
                LEFT.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = person_image[person_key];
                LEFT.GetComponentInChildren<TMP_Text>().text = dt.getCurrPerson();
                showHide("LEFT", true);
                showHide("RIGHT", false);
                break;
            case "RIGHT":
                RIGHT.transform.GetChild(0).GetComponent<UnityEngine.UI.Image>().sprite = person_image[person_key];
                RIGHT.GetComponentInChildren<TMP_Text>().text = dt.getCurrPerson();
                showHide("RIGHT", true);
                showHide("LEFT", false);
                break;
        }

    }

    private void showHide(string thing, bool isShow)
    {
        switch (thing)
        {
            case "LEFT":
                LEFT.SetActive(isShow);
                break;
            case "RIGHT":
                RIGHT.SetActive(isShow);
                break;
            default:
                dialogMaster.SetActive(isShow);
                LEFT.SetActive(true);
                RIGHT.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && Pause.paused == false)
        {
            continueLine();
        }
    }
}