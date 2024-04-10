using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasUI : MonoBehaviour
{
    private GameObject _gameController;
    private GameController _gameControllerScript;
    private Canvas _canvas;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.Find("GameController");
        _canvas = GetComponent<Canvas>();
        if (_gameController != null)
        {
            _gameControllerScript = _gameController.GetComponent<GameController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_gameControllerScript.GetGameWon() || _gameControllerScript.GetGameLost())
        {
            _canvas.enabled = false;
        }
    }
}
