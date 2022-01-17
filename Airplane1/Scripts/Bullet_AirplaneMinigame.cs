using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet_AirplaneMinigame : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    private void Start()
    {
        speed = 20f;
        rb.velocity = transform.right * speed;
        Destroy(gameObject, 2f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            Enemy_AirplaneMinigame.instance.currentHeal--;
            Destroy(gameObject);
        }
    }

}
