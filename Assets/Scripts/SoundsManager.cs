﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsManager : MonoBehaviour
{
    public static AudioClip playerjumpsound, playerhitsound, playerdeathsound;
    static AudioSource audioSrc;

    void Start()
    {
        playerjumpsound = Resources.Load<AudioClip>("jump");
        playerhitsound = Resources.Load<AudioClip>("hit");
        playerdeathsound = Resources.Load<AudioClip>("death");

        audioSrc = GetComponent<AudioSource> ();
    }

    public static void PlaySound (string clip)
    {
        switch (clip) 
        {
            case "jump":
                audioSrc.PlayOneShot(playerjumpsound);
                break;
            case "hit":
                audioSrc.PlayOneShot(playerhitsound);
                break;
            case "death":
                audioSrc.PlayOneShot(playerdeathsound);
                break;
        }
    }
}