using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float maxHealth = 10;
    private float health = 10;
    private int damage = 1;
    [SerializeField] private float moveSpeed = 3.5f;
    private bool _isKnockedBack = false;
    private bool _isFrozen = false;
    private bool _isBurning = false;
    
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.Find("Player");
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        // follow player
        if (!_isKnockedBack)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
                moveSpeed * Time.deltaTime);
        }
    }
    
    public Dictionary<String, String> ReturnStats() {
        Dictionary<String, String> stats = new Dictionary<String, String>();
        stats.Add("health", health.ToString());
        stats.Add("maxHealth", maxHealth.ToString());
        return stats;
    }
    
    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
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
            _spriteRenderer.color = Color.cyan;
            TakeDamage(2f);
            StartCoroutine(StopFreeze());
        }
    }

    IEnumerator StopFreeze()
    {
        yield return new WaitForSeconds(2f);
        _isFrozen = false;
        moveSpeed = 3.5f;
        _spriteRenderer.color = Color.yellow;
    }

    public void Burn()
    {
        _isBurning = true;
        StartCoroutine(Burning());
        _isBurning = false;
    }

    IEnumerator Burning()
    {
        for(int i = 0; i < 3; i++)
        {
            TakeDamage(1f);
            _spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            _spriteRenderer.color = Color.yellow;
            yield return new WaitForSeconds(0.5f);
        }
    }


}
