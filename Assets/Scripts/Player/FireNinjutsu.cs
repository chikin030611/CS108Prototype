using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FireNinjutsu : MonoBehaviour
{
    [SerializeField] private int kiCost = 1;
    [FormerlySerializedAs("_damage")] public float damage;
    
    private void OnDestroy()
    {
        Destroy(gameObject);
    }

    public int GetKiCost()
    {
        return kiCost;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Burn(damage);
            Debug.Log(damage);
        }
    }
}
