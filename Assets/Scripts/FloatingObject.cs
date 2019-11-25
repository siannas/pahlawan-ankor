using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingObject : MonoBehaviour
{

    //public float degreesPerSecond = 15.0f;
    public float amplitude = 0f;
    public float frequency = 0.5f;

    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    void Start()
    {
        posOffset = transform.position;
        transform.name = transform.name.Replace("(Clone)", "").Trim();
    }

    void Update()
    {
        //transform.Rotate(new Vector3(0f, Time.deltaTime * degreesPerSecond, 0f), Space.World);

        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameObject.name == "Medkit")
            {
                SoundsManager.PlaySound("powerup");
                Destroy(gameObject);
                Player.curHealth += 20;
            }
        }
    }
}
