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
    [SerializeField] private float moveSpeed = 10f;
    
    private GameObject _player;
    private Rigidbody2D _rigidbody;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_player == null)
        {
            _player = GameObject.Find("Player");
        }

        _rigidbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        // follow player
        transform.position = Vector2.MoveTowards(transform.position, _player.transform.position,
            moveSpeed * Time.deltaTime);
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
    
    private void OnTriggerEnter2D(Collider2D other)
    {
    }

    public void Knockback()
    {
        _rigidbody.velocity = Vector2.zero;
        float horizontal = transform.position.x - _player.transform.position.x;
        _rigidbody.AddForce(new Vector2(horizontal, 5f), ForceMode2D.Impulse);
    }
}
