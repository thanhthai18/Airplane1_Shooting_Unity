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
    private int isSpawnBG;
    public bool isIntro;
    public bool isWin;

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
        isWin = false;
        isIntro = true;
        if (isIntro)
        {
            mainCamera.DOOrthoSize(7, 5);
            mainCamera.transform.DOMove(new Vector3(0,0,-10), 5).OnComplete(() => { isIntro = false; });
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

    public void EndGame()
    {
        PopupManager.instance.ShowPopupEndMinigame(false);
    }

    public void WinGame()
    {
        PopupManager.instance.ShowPopupEndMinigame(true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            WinGame();
        }

        if(currBackGround.transform.position.x <= -50)
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
