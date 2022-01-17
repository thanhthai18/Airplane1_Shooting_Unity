using DG.Tweening;
using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_AirplaneMinigame : MonoBehaviour
{
    [SerializeField] private BulletEnemy_AirplaneMinigame bulletEnemyPrefab;
    [SerializeField] private Transform gunPoint;
    [SerializeField] private GameObject meteoritePrefab;
    [SerializeField] private List<Transform> meteoritePoint = new List<Transform>();
    [SerializeField] private GameObject effectBulletPrefab;
    public static Enemy_AirplaneMinigame instance;
    public float maxHeal;
    public float currentHeal;
    public HPBar_AirplaneMinigame hpBar;
    public int levelEnemy;
    public int levelMeteorite;
    public bool isLevel1;
    public bool isLevel2;
    public bool isMeteorite1;
    public bool isMeteorite2;
    public bool isTempDangerous;
    public Coroutine spawnBulletCorotine;
    public Coroutine spawnMeteoriteCorotine;
    public float xemHP = 1;
    public bool isJump;
    public bool isEnd;

    public SkeletonAnimation anim;
    [SpineAnimation] public string anim_Idle, anim_LenXuong, anim_MoMieng, anim_NhayMat;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(instance);
    }

    private void Start()
    {
        //anim.state.Complete += AnimComplete;
        //PlayAnim(anim, anim_Idle, true);

        isEnd = false;
        isJump = true;
        isTempDangerous = true;
        isLevel1 = false;
        isMeteorite1 = false;
        isLevel2 = false;
        isMeteorite2 = false;
        levelEnemy = 0;
        levelMeteorite = 0;
        maxHeal = 500f; //1900
        currentHeal = maxHeal;

        hpBar.max_hp = maxHeal;
        hpBar.current_hp = currentHeal;
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

    void Jump()
    {
        Sequence jumpSeq = DOTween.Sequence();
        jumpSeq.Append(transform.DOMoveY(transform.position.y + 4, 2f));
        jumpSeq.Append(transform.DOMoveY(transform.position.y - 3, 2f));
        jumpSeq.Append(transform.DOMoveY(transform.position.y, 1f));
        jumpSeq.SetLoops(-1);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Path"))
        {
            var bulletFX = Instantiate(effectBulletPrefab, collision.transform.position, Quaternion.identity);
            Destroy(bulletFX, 0.05f);
        }
    }

    void OnDead()
    {
        Airplane_AirphaneMinigame.instance.GetComponent<Collider2D>().enabled = false;
        GameController_AirplaneMinigame.instance.Win();
        hpBar.gameObject.SetActive(false);
        GetComponent<PolygonCollider2D>().enabled = false;
        GetComponent<SpriteRenderer>().DOFade(0, 4);
        transform.DOMoveY(transform.position.y - 5, 5).OnComplete(() =>
        {
            Destroy(gameObject);
        });
    }



    private void Update()
    {
        xemHP = currentHeal / maxHeal;
        hpBar.current_hp = currentHeal;
        hpBar.ShowHPBar();

        if (currentHeal <= 0)
        {
            if (!isEnd)
            {
                isEnd = true;
                levelEnemy = -1;
                levelMeteorite = -1;
                OnDead();
                StopCoroutine(spawnBulletCorotine);
                StopCoroutine(spawnMeteoriteCorotine);
            }


        }

        if (currentHeal / maxHeal <= 0.95f && currentHeal / maxHeal > 0.9f)
        {
            levelEnemy = 1;
            isLevel1 = true;
        }

        if (currentHeal / maxHeal <= 0.9f && currentHeal / maxHeal > 0.5f)
        {
            levelEnemy = 2;
            isLevel1 = false;
            isLevel2 = true;

            if (isJump)
            {
                isJump = false;
                Jump();
            }
        }

        if (currentHeal / maxHeal <= 0.5f && currentHeal / maxHeal > 0.1f)
        {
            levelMeteorite = 1;
            isMeteorite1 = true;
        }

        if (currentHeal / maxHeal <= 0.1f && currentHeal / maxHeal > 0f)
        {
            levelMeteorite = 2;
            isMeteorite1 = false;
            isMeteorite2 = true;
        }

        if (levelEnemy == 1 && isTempDangerous)
        {
            spawnBulletCorotine = StartCoroutine(SpawnBulletEnemy());
            isTempDangerous = false;
        }

        if (levelMeteorite == 1 && !isTempDangerous)
        {
            spawnMeteoriteCorotine = StartCoroutine(SpawnMeteorite());
            isTempDangerous = true;
        }
    }

    IEnumerator SpawnBulletEnemy()
    {
        Instantiate(bulletEnemyPrefab, gunPoint.position, Quaternion.identity);
        while (isLevel1)
        {
            yield return new WaitForSeconds(4);
            Instantiate(bulletEnemyPrefab, gunPoint.position, Quaternion.identity);
            if (levelEnemy == 2)
            {
                isLevel1 = false;
            }
        }

        while (isLevel2)
        {
            yield return new WaitForSeconds(2f);
            Instantiate(bulletEnemyPrefab, gunPoint.position, Quaternion.identity);
            if (levelEnemy == -1)
            {
                isLevel2 = false;
            }
        }
    }

    IEnumerator SpawnMeteorite()
    {
        Instantiate(meteoritePrefab, meteoritePoint[1]);
        while (isMeteorite1)
        {
            yield return new WaitForSeconds(3);
            int random = Random.Range(0, 3);
            Instantiate(meteoritePrefab, meteoritePoint[random]);
            if (levelMeteorite == 2)
            {
                isMeteorite1 = false;
            }
        }

        while (isMeteorite2)
        {
            yield return new WaitForSeconds(1);
            int random = Random.Range(0, 3);
            Instantiate(meteoritePrefab, meteoritePoint[random]);
            if (levelMeteorite == -1)
            {
                isMeteorite2 = false;
            }
        }
    }
    // Fade shader material
    //IEnumerator FadeAlphaToZero(Renderer renderer)
    //{

    //    Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
    //    float duration = 0.5f;

    //    while (true)
    //    {

    //        for (int i = 0; i < renderer.materials.Length; i++)
    //        {
    //            float lerp = Mathf.PingPong(Time.time, duration) / duration;
    //            renderer.materials[i].color = Color.Lerp(startColor, endColor, lerp);

    //        }
    //        yield return null;
    //    }
    //}
    //IEnumerator Fade(Renderer renderer)
    //{
    //    StartCoroutine(coroutine);

    //    yield return new WaitForSeconds(2.5f);
    //    for (int i = 0; i < renderer.materials.Length; i++)
    //    {
    //        renderer.materials[i].color = startColor;
    //    }

    //    fade = false;
    //    Debug.Log("Lose Fade");
    //    StopCoroutine(coroutine);

    //}




}
