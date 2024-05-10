using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    private Slider _slider;
    private GameObject _boss;
    private Enemy _enemyScript;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _boss = GameObject.Find("Boss");
        _enemyScript = _boss.GetComponent<Enemy>();
    }
    
    void Update()
    {
        _slider.maxValue = _enemyScript.GetMaxHealth();
        _slider.value = _enemyScript.GetHealth();
    }
}
