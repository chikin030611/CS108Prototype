using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupTextUI : MonoBehaviour
{
    private GameObject player;
    private TextMeshProUGUI textObject;
    private PlayerControls _playerControls;
    
    private bool _isCoolDown;
    private int _ki;
    private float _alpha = 1f;
    public string _text = "";
    private Coroutine _currentCoroutine;

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
        if (_text.Equals("No ki"))
        {
            
            _ki = _playerControls.GetKi();
            if (_ki <= 0)
            {
                textObject.text = _text;
                StartDecreaseAlphaCoroutine();
            } 
            else
            {
                textObject.text = "";
            }
        }
        else
        {
            _isCoolDown = _text.Equals("Ninjutsu CD")
                ? _playerControls.GetIsNinjutsuCoolDown()
                : _playerControls.GetIsShurikenCoolDown();

            if (_isCoolDown)
            {
                textObject.text = _text + " \n";
                StartDecreaseAlphaCoroutine();
            }
            else
            {
                textObject.text = "";
            }
        }
    }
    
    private IEnumerator DecreaseAlpha()
    {
        while (_alpha > 0f)
        {
            _alpha -= 0.02f;
            textObject.color = new Color(1, 0, 0.275f, _alpha);
            yield return new WaitForSeconds(0.1f);
        }
        _alpha = 1f;
    }
    
    private void StartDecreaseAlphaCoroutine()
    {
        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }
        _currentCoroutine = StartCoroutine(DecreaseAlpha());
    }
}
