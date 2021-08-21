using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Meteorite_AirplaneMinigame : MonoBehaviour
{

    private void Start()
    {
        transform.DOMove(new Vector2(transform.position.x - 40, transform.position.y - 40), 9);
        Destroy(gameObject, 4f);
    }
}
