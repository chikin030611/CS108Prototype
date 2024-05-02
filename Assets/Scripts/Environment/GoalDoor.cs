using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalDoor : MonoBehaviour
{
    
    private GameObject _gameController;
    private GameController _gameControllerScript;
    private BoxCollider2D _boxCollider2D;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameController = GameObject.Find("GameController");
        _boxCollider2D = GetComponent<BoxCollider2D>();
        if (_gameController != null)
        {
            _gameControllerScript = _gameController.GetComponent<GameController>();
        }
    }
    
    void Update()
    {
        if (_boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            if (Input.GetButtonDown("Jump"))
            { 
                _gameControllerScript.TransferToNextLevel();
            }
        }
    }
    
}