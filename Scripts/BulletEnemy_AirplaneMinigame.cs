using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletEnemy_AirplaneMinigame : MonoBehaviour
{
    public float speed;
    public Rigidbody2D rb;

    private void Start()
    {
        speed = 10f;
        rb.velocity = -transform.right * speed;
        Destroy(gameObject, 2f);
    }

    
}
