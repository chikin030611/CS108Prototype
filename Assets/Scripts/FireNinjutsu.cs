using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireNinjutsu : MonoBehaviour
{
    [SerializeField] private float damage = 2f;
    [SerializeField] private int kiCost = 1;
    
    public int GetKiCost()
    {
        return kiCost;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Burn();
        }
    }
}
