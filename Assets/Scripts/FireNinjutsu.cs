using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNinjutsu : MonoBehaviour
{
    [SerializeField] private float _damage = 1f;
    [SerializeField] private int kiCost = 1;

    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    public int GetKiCost()
    {
        return kiCost;
    }
    
    public float GetDamage()
    {
        return _damage;
    }
    
    public void SetDamage(float newDamage)
    {
        _damage = newDamage;
    }
    
    public void LevelUp()
    {
        _damage += .2f;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Burn(_damage);
            Debug.Log(_damage);
        }
    }
}
