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
        IsTweening = false;
    }
    void Update()
    {
        Health.text = Player.curHealth.ToString() + "%";

        if (Player.curHealth <= 30 && !IsTweening)
        {
            IsTweening = true;
            DOTween.Restart(Hp);
            Hp.DOScale(new Vector3 (1.2f,1.2f,1.2f), 0.5f).SetLoops(-1, LoopType.Yoyo);
        }
        if (Player.curHealth > 30)
        {
            Hp.DOScale(new Vector3(1f, 1f, 1f), 0.5f);
            IsTweening = false;
        }
    }
}
