using DG.Tweening;
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
        Invoke(nameof(DelayCallisWin), 2f);
        hpBar.gameObject.SetActive(false);
        transform.DOMoveY(transform.position.y - 5, 5).OnComplete(() =>
        {
            Destroy(gameObject);

        });
    }

    void DelayCallisWin()
    {
        GameController_AirplaneMinigame.instance.isWin = true;
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
            yield return new WaitForSeconds(2);
            Instantiate(bulletEnemyPrefab, gunPoint.position, Quaternion.identity);
            if (levelEnemy == 2)
            {
                isLevel1 = false;
            }
        }

        while (isLevel2)
        {
            yield return new WaitForSeconds(1f);
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




}
