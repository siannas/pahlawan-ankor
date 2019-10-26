using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class HealthUI : MonoBehaviour
{
    public Text Health;
    public Transform Hp;
    private bool IsTweening;

    void Start()
    {
        IsTweening = true;
    }
    void Update()
    {
        Health.text = Player.curHealth.ToString() + "%";

        if (Input.GetKeyDown(KeyCode.I)) //Tes
        {
            Player.curHealth -= 10;
        }

        if (Player.curHealth <= 10 && IsTweening)
        {
            IsTweening = false;
            Hp.DOScale(new Vector3 (1.2f,1.2f,1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
    }


}
