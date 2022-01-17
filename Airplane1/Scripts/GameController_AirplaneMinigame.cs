using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DemoObserver;

public class GameController_AirplaneMinigame : MonoBehaviour
{
    public static GameController_AirplaneMinigame instance;

    [SerializeField] private GameObject currBackGround;
    [SerializeField] private GameObject nextBackGround;
    [SerializeField] private Camera mainCamera;
    public Airplane_AirphaneMinigame airplaneObj;
    private int isSpawnBG;
    public bool isIntro;
    public bool isWin, isLose;
    public float startSizeCamera;

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
        startSizeCamera = mainCamera.orthographicSize;
        SetSizeCamera();
        mainCamera.orthographicSize = (3.0f / 7) * startSizeCamera;
        isWin = false;
        isIntro = true;
        if (isIntro)
        {
            mainCamera.DOOrthoSize(startSizeCamera, 3);
            mainCamera.transform.DOMove(new Vector3(0, 0, -10), 5).OnComplete(() => { isIntro = false; });
        }
        isSpawnBG = 0;
        currBackGround = Instantiate(currBackGround, new Vector3(26.4f, 2.824244f, 0.417622f), Quaternion.identity);
        MoveBG();
    }

    void SetSizeCamera()
    {
        float f1 = 16.0f / 9;
        float f2 = Screen.width * 1f / Screen.height;
        mainCamera.orthographicSize *= f2 / f1;
    }

    void DestroyBG()
    {
        Destroy(currBackGround);
        isSpawnBG = 0;
        currBackGround = nextBackGround;
    }

    void MoveNextBG()
    {
        nextBackGround.transform.DOMoveX(-100f, 55f).SetEase(Ease.Flash);
    }

    void MoveBG()
    {
        currBackGround.transform.DOMoveX(-100f, 47f).SetEase(Ease.Flash);
    }

    public void Win()
    {
        isWin = true;
        Debug.Log("Win");
        StopAllCoroutines();
        airplaneObj.StopAllCoroutines();
        airplaneObj.GetComponent<PolygonCollider2D>().enabled = false;
        mainCamera.DOOrthoSize((3.0f / 7) * startSizeCamera, 2f);
        mainCamera.transform.DOMove(new Vector3(airplaneObj.transform.position.x, airplaneObj.transform.position.y, -10), 2f).OnComplete(() =>
        {
            mainCamera.transform.DOMove(new Vector3(0, 0, -10), 3f);
            mainCamera.DOOrthoSize(startSizeCamera, 3f).SetEase(Ease.Linear).OnComplete(() =>
            {
                airplaneObj.transform.DOMove(new Vector3(transform.position.x + 30, -0.2f, 0), 5f);
            });

        });

    }

    public void Lose()
    {
        isLose = true;
        Debug.Log("Thua");
        StopAllCoroutines();
    }


    private void Update()
    {
        if (currBackGround.transform.position.x <= -50)
        {
            isSpawnBG++;
        }

        if (isSpawnBG == 1)
        {
            //nextBackGround = Instantiate(currBackGround, new Vector3(-(currBackGround.transform.position.x - 5f), 2.824244f, 0.417622f), Quaternion.identity);
            nextBackGround = Instantiate(currBackGround, new Vector3((currBackGround.transform.position.x + 105.3f), 2.824244f, 0.417622f), Quaternion.identity);
            MoveNextBG();
            Invoke("DestroyBG", 12f);
            isSpawnBG++;
        }
    }


}
