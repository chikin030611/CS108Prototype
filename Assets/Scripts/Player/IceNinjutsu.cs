using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class IceNinjutsu : MonoBehaviour
{
    private float lifeTime = 2f;
    [SerializeField] private int kiCost = 1;
    public bool direction;
    public float freezeTime;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update()
    {
        Flip();
    }
    
    public int GetKiCost()
    {
        return kiCost;
    }
    
    private void Flip()
    {
        if (direction)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().Freeze(freezeTime);
        }
    }

}
