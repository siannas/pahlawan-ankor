using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using DG.Tweening;
using KoganeUnityLib;

public class DialogueManager : MonoBehaviour
{
    public GameObject person1;
    public GameObject person2;
    public GameObject dialogBox;
    
    GameObject dialogMaster;
    TextMeshProUGUI dialogLine;

    public TMP_Typewriter m_typewriter;
    public float m_speed;

    bool complete = true;

    private dialogueTemplate dt;

    void Start()
    {
        dialogMaster = gameObject;
        dialogLine = dialogBox.GetComponentInChildren<TextMeshProUGUI>();
        dt = gameObject.GetComponent<dialogueTemplate>();
        dt.startScenarioAt(0);
        continueLine();
    }

    private void showDialogLine(string line)
    {
        complete = false;
        m_typewriter.Play
        (
            text: line,
            speed: m_speed,
            onComplete: () => complete=true
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
            showDialogLine(line);
        }
    }

    private void showHide(string thing, bool isShow)
    {
        switch (thing)
        {
            case "person1":
                person1.SetActive(isShow);
                break;
            case "person2":
                person2.SetActive(isShow);
                break;
            default:
                dialogMaster.SetActive(isShow);
                dialogLine.SetText("");
                person1.SetActive(true);
                person2.SetActive(true);
                break;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            continueLine();
        }
    }
}