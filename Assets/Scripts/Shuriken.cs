using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shuriken : MonoBehaviour
{
    private float moveSpeed;
    
    private CircleCollider2D shurikenCollider;
    private SpriteRenderer sr;
    private Projectile projectile;
    
    // Start is called before the first frame update
    void Start()
    {
        shurikenCollider = GetComponent<CircleCollider2D>();
        sr = transform.GetChild(0).GetComponent<SpriteRenderer>();
        projectile = GetComponent<Projectile>();
        moveSpeed = projectile.moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (shurikenCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            projectile.moveSpeed = 0;
            sr.transform.Rotate(0, 0, 0);
        }
        else
        {
            sr.transform.Rotate(0, 0, 1);
        }

    }
    
}
