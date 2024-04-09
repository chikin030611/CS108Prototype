using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EnemyStatUI : MonoBehaviour
{
    private TextMeshProUGUI textObject;
    private Dictionary<String, String> enemyStats;
    private Enemy enemyScript;
    
    // Start is called before the first frame update
    void Start()
    {
        GameObject enemy = transform.parent.gameObject.transform.parent.gameObject;
        enemyScript = enemy.GetComponent<Enemy>();
        textObject = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // get enemy stats from parents
        enemyStats = enemyScript.ReturnStats();
        String enemyHP = enemyStats["health"];
        String maxEnemyHP = enemyStats["maxHealth"];
        textObject.text = "HP: " + enemyHP + "/" + maxEnemyHP;
    }
}
