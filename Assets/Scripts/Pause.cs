using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Pause : MonoBehaviour {

    public GameObject PauseUI;
    private DialogueManager Dialogmanager;
    private bool dialogcomplete;
    private string main;
    
    public static bool paused = false;

    void Start()
    {
        PauseUI.SetActive(false);
        if (main == SceneManager.GetActiveScene().name)
        {
            Dialogmanager = GameObject.Find("dialogueMaster").GetComponent<DialogueManager>();
        }
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
        Application.Quit();
    }
}
