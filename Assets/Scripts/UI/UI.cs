using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI : MonoBehaviour
{
    // Character Variables
    private int health;
    private int maxHealth;
    private int ki;
    private int maxKi;
    private int level;
    private int exp;
    private int expToLevelUp;
    private float ninjutsuCoolDownTime;
    private float shurikenCoolDownTime;
    
    private GameObject player;
    private TextMeshProUGUI textObject;
    private PlayerControls _playerControls;
    
    // Start is called before the first frame update
    void Start()
    {
        // Getting the player's health and ki
        player = GameObject.Find("Player");
        if (player != null)
        {
            _playerControls = player.GetComponent<PlayerControls>();
        }
        textObject = GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        health = _playerControls.GetHealth();
        maxHealth = _playerControls.GetMaxHealth();
        ki = _playerControls.GetKi();
        maxKi = _playerControls.GetMaxKi();
        level = _playerControls.GetLevel();
        exp = _playerControls.GetExp();
        expToLevelUp = _playerControls.GetExpToLevelUp();
        ninjutsuCoolDownTime = _playerControls.GetNinjutsuCoolDownTime();
        shurikenCoolDownTime = _playerControls.GetShurikenCoolDownTime();
        
        textObject.text = "<color=#FF0000>HP:</color> " + health + "/" + maxHealth + "\n" +
                          "<color=#00BDFF>Ki:</color> " + ki + "/" + maxKi + "\n" +
                          "<color=#FFC900>Level:</color> " + level + "\n" +
                          "<color=#FFC900>Exp:</color> " + exp + "/" + expToLevelUp + "\n" +
                          "<color=#FF0000>Ninjutsu Cooldown:</color> " + ninjutsuCoolDownTime.ToString("F1") + "\n" +
                          "<color=#FF0000>Shuriken Cooldown:</color> " + shurikenCoolDownTime.ToString("F1");
        
        
        // DebugUI();
        
    }
    
    
    // Debug
    void DebugUI()
    {
        Dictionary<String, String> debug = _playerControls.DebugDictionary();
        String debugText = "";
        foreach (KeyValuePair<String, String> entry in debug)
        {
            debugText += entry.Key + ": " + entry.Value + "\n";
        }
        textObject.text += debugText;
    }


}