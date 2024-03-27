using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.ReorderableList;
using UnityEngine;

public class UI : MonoBehaviour
{
    // Character Variables
    private int health;
    private int maxHealth;
    private int ki;
    private int maxKi;
    
    private GameObject player;
    private TextMeshProUGUI textObject;
    private PlayerMovement playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        // Getting the player's health and ki
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        textObject = GetComponent<TextMeshProUGUI>();
    }
    
    // Update is called once per frame
    void Update()
    {
        health = playerMovement.GetHealth();
        maxHealth = playerMovement.GetMaxHealth();
        ki = playerMovement.GetKi();
        maxKi = playerMovement.GetMaxKi();

        // textObject.text = "Health: " + health + "/" + maxHealth + "\n" +
        //                   "Ki: " + ki + "/" + maxKi + "\n";
        
        DebugUI();
        
    }
    
    
    // Debug
    void DebugUI()
    {
        Dictionary<String, String> debug = playerMovement.DebugDictionary();
        String debugText = "";
        foreach (KeyValuePair<String, String> entry in debug)
        {
            debugText += entry.Key + ": " + entry.Value + "\n";
        }
        textObject.text = debugText;
    }


}
