using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuEvent : MonoBehaviour
{

    private SpriteRenderer Fader;
    public GameObject VolumeUI;
    private bool ShowingPengaturan = false;
    public AudioSource audiosrc;
    public Slider slider;
    private float audiovol;

    void Start()
    {
        Time.timeScale = 1f;
        Fader = GameObject.Find("Fader").GetComponent<SpriteRenderer>();
        StartCoroutine(Fading(true));
        audiovol = PlayerPrefs.GetFloat("Options_VolumeLevel");
        slider.value = audiovol;
    }

    void Update()
    {
        audiosrc.volume = audiovol;
        PlayerPrefs.SetFloat("Options_VolumeLevel", audiovol);
    }

    public void Mulai()
    {
        SceneManager.LoadScene(1);
    }

    public void Pengaturan()
    {
        if (ShowingPengaturan)
        {
            VolumeUI.SetActive(false);
            ShowingPengaturan = false;
        }

        else
        {
            VolumeUI.SetActive(true);
            ShowingPengaturan = true;
        }

        Debug.Log(ShowingPengaturan);
    }

    public void Keluar()
    {
        Application.Quit();
    }

    public void Setvol(float vol)
    {
        audiovol = vol;
    }

    IEnumerator Fading(bool fading)
    {
        
        if (fading)
        {
            for (float i = 1; i >= 0; i -= Time.deltaTime)
            {
                Fader.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }
        else
        {
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                Fader.color = new Color(0, 0, 0, i);
                yield return null;
            }
        }

    }
}
