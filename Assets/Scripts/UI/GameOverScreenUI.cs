using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverScreenUI : MonoBehaviour
{
    public string _gameOverText;
    public string _gameOverTextWithColor;
    private TextMeshProUGUI textObject;

    // Start is called before the first frame update
    void Start()
    {
        textObject = GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        _gameOverTextWithColor = "<color=#FFC900>" + _gameOverText + "</color>\n Press R to restart";
        textObject.text = _gameOverTextWithColor;
    }
}
