using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    private float _health;
    private int damage = 1;
    [SerializeField] private float moveSpeed = 3.5f;
    private bool _isKnockedBack = false;
    private bool _isFrozen = false;
    private bool _isBurning = false;
    private bool _isFacingRight = false;
    private bool _isFoundPlayer = false;
    
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    
    private SpriteRenderer _burningSpriteRenderer;
    private Collider2D _playerDetector;
    
    private GameObject gameController;
    private GameController gameControllerScript;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _burningSpriteRenderer = transform.GetChild(2).GetComponent<SpriteRenderer>();
        _playerDetector = transform.GetChild(3).GetComponent<Collider2D>();
        
        _player = GameObject.Find("Player");
        gameController = GameObject.Find("GameController");
        if (gameController != null)
        {
            gameControllerScript = gameController.GetComponent<GameController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        _isFacingRight = transform.position.x > _player.transform.position.x;
        _spriteRenderer.flipX = _isFacingRight;
        
        if (_playerDetector.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            _isFoundPlayer = true;
        }
        
        // follow player
        if (!_isKnockedBack && _isFoundPlayer)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                moveSpeed * Time.deltaTime);
        }
    }
    
    public Dictionary<String, String> ReturnStats() {
        Dictionary<String, String> stats = new Dictionary<String, String>();
        stats.Add("health", _health.ToString());
        stats.Add("maxHealth", maxHealth.ToString());
        return stats;
    }
    
    public void TakeDamage(float damage)
    {
        _health -= damage;
        _spriteRenderer.color = Color.red;
        StartCoroutine(FlashRed());
        if (_health <= 0)
        {
            int numOfEnemies = gameControllerScript.ReturnNumOfEnemies();
            gameControllerScript.UpdateNumOfEnemies(--numOfEnemies); // Update the number of enemies
            Destroy(gameObject);
        }
    }
    
    IEnumerator FlashRed()
    {
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = Color.white;
    }

    public void KnockBack()
    {
        _isKnockedBack = true;
        _rigidbody.velocity = Vector2.zero;
        float horizontal = transform.position.x - _player.transform.position.x;
        _rigidbody.AddForce(new Vector2(horizontal * 8f, 5f), ForceMode2D.Impulse);
        StartCoroutine(StopKnockback());
    }
    
    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.5f);
        _isKnockedBack = false;
    }

    public void Freeze()
    {
        if (!_isFrozen)
        {
            _isFrozen = true;
            moveSpeed = 0;
            _health -= damage;
            _spriteRenderer.color = Color.cyan;
            if (_health <= 0)
            {
                int numOfEnemies = gameControllerScript.ReturnNumOfEnemies();
                gameControllerScript.UpdateNumOfEnemies(--numOfEnemies); // Update the number of enemies
                Destroy(gameObject);
            }            
            StartCoroutine(StopFreeze());
        }
    }

    IEnumerator StopFreeze()
    {
        yield return new WaitForSeconds(2f);
        _isFrozen = false;
        moveSpeed = 3.5f;
        _spriteRenderer.color = Color.white;
    }

    public void Burn()
    {
        _isBurning = true;
        StartCoroutine(Burning());
        _isBurning = false;
    }

    IEnumerator Burning()
    {
        _burningSpriteRenderer.enabled = true;   
        for(int i = 0; i < 3; i++)
        {
            TakeDamage(1f);
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = Color.white;
            yield return new WaitForSeconds(0.5f);
        }
        _burningSpriteRenderer.enabled = false;
    }


}
