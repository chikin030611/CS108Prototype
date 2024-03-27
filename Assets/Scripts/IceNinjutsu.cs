using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceNinjutsu : MonoBehaviour
{
    private float lifeTime = 2f;
    public bool direction;
    
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
}
