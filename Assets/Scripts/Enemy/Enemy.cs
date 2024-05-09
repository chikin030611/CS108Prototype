using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    private float _health;
    private int damage = 1;
    [SerializeField] private int exp = 1;
    [SerializeField] private float moveSpeed = 3.5f;
    private bool _isKnockedBack = false;
    public bool isFrozen = false;
    private bool _isBurning = false;
    public bool isFacingRight = false;
    public bool isFoundPlayer = false;
    
    private GameObject _player;
    private PlayerControls _playerControls;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    
    private ParticleSystem _burningParticleSystem;
    private Collider2D _playerDetector;
    
    private GameObject gameController;
    private GameController gameControllerScript;
    
    private Color _originalColor;
    
    // Start is called before the first frame update
    void Start()
    {
        _health = maxHealth;
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _burningParticleSystem = GetComponent<ParticleSystem>();
        _burningParticleSystem.Stop();
        _playerDetector = transform.GetChild(2).GetComponent<Collider2D>();
        
        _originalColor = _spriteRenderer.color;
        
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerControls = _player.GetComponent<PlayerControls>();
        }
        gameController = GameObject.Find("GameController");
        if (gameController != null)
        {
            gameControllerScript = gameController.GetComponent<GameController>();
        }

    }

    // Update is called once per frame
    void Update()
    {
        isFacingRight = transform.position.x > _player.transform.position.x;
        _spriteRenderer.flipX = isFacingRight;
        
        if (_playerDetector.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            isFoundPlayer = true;
        }
        
        // follow player
        if (isFoundPlayer)
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
            Destroy(gameObject);
            _playerControls.AddExp(exp);
        }
    }
    
    IEnumerator FlashRed()
    {
        yield return new WaitForSeconds(0.1f);
        _spriteRenderer.color = _originalColor;
    }

    public void KnockBack()
    {
        _isKnockedBack = true;
        _rigidbody.velocity = Vector2.zero;
        moveSpeed = 0;
        float horizontal = transform.position.x - _player.transform.position.x;
        _rigidbody.AddForce(new Vector2(horizontal * 8f, 5f), ForceMode2D.Impulse);
        StartCoroutine(StopKnockback());
    }
    
    IEnumerator StopKnockback()
    {
        yield return new WaitForSeconds(0.3f);
        moveSpeed = 3.5f;
        _isKnockedBack = false;
    }

    public void Freeze(float freezeTime)
    {
        if (!isFrozen)
        {
            isFrozen = true;
            moveSpeed = 0;
            _health -= damage;
            _spriteRenderer.color = Color.cyan;
            if (_health <= 0)
            {
                Destroy(gameObject);
            }
            
            StartCoroutine(StopFreeze(freezeTime));
        }
    }

    IEnumerator StopFreeze(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        isFrozen = false;
        moveSpeed = 3.5f;
        _spriteRenderer.color = _originalColor;
    }

    public void Burn(float damage)
    {
        _isBurning = true;
        StartCoroutine(Burning(damage));
        _isBurning = false;
    }

    IEnumerator Burning(float damage)
    {
        _burningParticleSystem.Play();
        for(int i = 0; i < 3; i++)
        {
            TakeDamage(damage);
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = _originalColor;
            yield return new WaitForSeconds(0.5f);
        }
        _burningParticleSystem.Stop();
    }


}
