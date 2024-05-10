using System;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<PlayerControls>().TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
