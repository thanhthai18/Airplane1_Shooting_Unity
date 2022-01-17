using DG.Tweening;
using Spine.Unity;
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
    public bool isDead;
    [SerializeField] Camera mainCamera;
    private float maxXCamera;
    private float maxYCamera;

    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Idle, anim_Thang, anim_Thua;


    private void Start()
    {
        //anim.state.Complete += AnimComplete;
        Invoke(nameof(SetUpMap), 5.5f);
    }

    private void AnimComplete(Spine.TrackEntry trackEntry)
    {
        //if (trackEntry.Animation.Name == anim_BellRun)
        //{
        //    PlayAnim(anim, anim_Run, true);
        //}
    }

    public void PlayAnim(SkeletonAnimation anim, string nameAnim, bool loop)
    {
        anim.state.SetAnimation(0, nameAnim, loop);
    }

    void SetUpMap()
    {
        maxXCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).x;
        maxYCamera = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)).y;
        isHoldMouse = false;
        isShoot = true;
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
        GetComponent<PolygonCollider2D>().enabled = false;
        Destroy(gameObject, 10);
        GameController_AirplaneMinigame.instance.Lose();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!GameController_AirplaneMinigame.instance.isWin)
        {
            if ((collision.CompareTag("Finish") || collision.CompareTag("Trash")) && !isDead)
            {
                if (collision.CompareTag("Finish"))
                {
                    Destroy(collision.gameObject);
                }
                isDead = true;
                isShoot = false;
                var colFX = Instantiate(colliderFX, gameObject.transform.position, Quaternion.identity);
                Destroy(colFX, 0.5f);
                OnDead();
            }
        }

    }


    void Update()
    {
        if (!GameController_AirplaneMinigame.instance.isIntro)
        {
            if (!GameController_AirplaneMinigame.instance.isWin && !GameController_AirplaneMinigame.instance.isLose)
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
    }

}
