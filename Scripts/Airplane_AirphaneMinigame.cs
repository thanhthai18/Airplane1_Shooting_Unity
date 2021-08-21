using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane_AirphaneMinigame : MonoBehaviour
{
    public static Airplane_AirphaneMinigame instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);
    }

    [SerializeField] private GameObject colliderFX;
    public Vector2 mouseCurrentPos;
    public bool isHoldMouse;
    public Transform gunPoint;
    public GameObject bulletPrefab;
    private bool isShoot;
    public bool isFly;
    public bool isDead;
    [SerializeField] Camera mainCamera;
    private float maxXCamera;
    private float maxYCamera;


    private void Start()
    {
        Invoke(nameof(SetUpMap), 5.5f);
    }

    void SetUpMap()
    {
        maxXCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        maxYCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        isHoldMouse = false;
        isShoot = true;
        isFly = true;
        isDead = false;
        StartCoroutine(Shoot());
    }

    void Move()
    {
        transform.DOMove(new Vector3(mouseCurrentPos.x, mouseCurrentPos.y, 0), 0.1f);
        //transform.position = new Vector3(
        //        Mathf.Clamp(transform.position.x, -maxXCamera, maxXCamera),
        //        Mathf.Clamp(transform.position.y, -maxYCamera, maxYCamera),
        //        transform.position.z);
    }

    IEnumerator Shoot()
    {
        while (isShoot)
        {
            yield return new WaitForSeconds(0.05f);
            Instantiate(bulletPrefab, gunPoint.position, gunPoint.rotation);
        }
    }

    void OnDead()
    {
        isDead = true;
        transform.DOMove(new Vector2(transform.position.x + 2, transform.position.y - 14), 8);
        Enemy_AirplaneMinigame.instance.StopAllCoroutines();
        Destroy(gameObject, 10);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.CompareTag("Finish") || collision.CompareTag("Trash")) && !isDead)
        {
            if (collision.CompareTag("Finish"))
            {
                Destroy(collision.gameObject);
            }
            isDead = true;
            isShoot = false;
            isFly = false;
            var colFX = Instantiate(colliderFX, gameObject.transform.position, Quaternion.identity);
            Destroy(colFX, 0.5f);
            OnDead();
        }
    }


    void Update()
    {
        if (!GameController_AirplaneMinigame.instance.isIntro)
        {
            if (isFly)
            {
                if (Input.GetMouseButtonDown(0))
                {

                    isHoldMouse = true;

                }
                if (Input.GetMouseButtonUp(0))
                {
                    isHoldMouse = false;
                }

                if (isHoldMouse)
                {
                    mouseCurrentPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
                    mouseCurrentPos = new Vector2(Mathf.Clamp(mouseCurrentPos.x, -maxXCamera + 1.8f, maxXCamera - 1.8f), Mathf.Clamp(mouseCurrentPos.y, -maxYCamera + 1, maxYCamera - 1));
                    Move();
                }
            }
        }

        if (GameController_AirplaneMinigame.instance.isWin)
        {
            GameController_AirplaneMinigame.instance.isWin = false;
            StopAllCoroutines();
            isFly = false;
            mainCamera.DOOrthoSize(3, 2f);
            mainCamera.transform.DOMove(new Vector3(transform.position.x, transform.position.y, -10), 2f).OnComplete(() =>
            {
                mainCamera.transform.DOMove(new Vector3(0, 0, -10), 3f);
                mainCamera.DOOrthoSize(7, 3f).SetEase(Ease.Linear).OnComplete(() =>
                {
                    transform.DOMove(new Vector3(transform.position.x + 30, -0.2f, 0), 5f);

                });

            });

        }




    }

}
