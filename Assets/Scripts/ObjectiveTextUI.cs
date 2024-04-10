using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectiveTextUI : MonoBehaviour
{
    private TextMeshProUGUI textObject;
    private GameObject gameController;
    private GameController gameControllerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponent<TextMeshProUGUI>();
        gameController = GameObject.Find("GameController");
        if (gameController != null)
        {
            gameControllerScript = gameController.GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        textObject.text = gameControllerScript.ReturnNumOfEnemies().ToString();
    }
}
