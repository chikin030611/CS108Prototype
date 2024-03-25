using System;
using UnityEngine;

public class Potion : MonoBehaviour
{
    private int heal = 1;
    
    private GameObject player;
    private PlayerMovement playerMovement;
    [SerializeField] private bool isHealthPotion = true;
    
    private Collider2D potionCollider;
    
    // Start is called before the first frame update
    void Start()
    {   
        player = GameObject.Find("Player");
        if (player != null)
        {
            playerMovement = player.GetComponent<PlayerMovement>();
        }
        potionCollider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (potionCollider.IsTouching(player.GetComponent<Collider2D>()))
        {
            playerMovement.Heal(isHealthPotion, heal);
            Destroy(gameObject);
        }
    }
}
