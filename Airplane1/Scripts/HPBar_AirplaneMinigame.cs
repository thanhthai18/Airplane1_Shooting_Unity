using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar_AirplaneMinigame : MonoBehaviour
{
    public Image Bar;
    public float max_hp;
    public float current_hp;


    private void Start()
    {
        gameObject.SetActive(false);
        Bar.fillAmount = current_hp / max_hp;
        Invoke(nameof(SetActiveHPBar), 5);
    }

    void SetActiveHPBar()
    {
        gameObject.SetActive(true);
    }

    public void ShowHPBar()
    {
        if (current_hp < 0)
        {
            current_hp = 0;
        }
        Bar.fillAmount = current_hp / max_hp;
    }
}
