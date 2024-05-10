using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossFight : MonoBehaviour
{
    private bool _enemyIsFrozen;
    private float _playerFreezeTime;
    private float _attackInterval = 3f;
    
    public GameObject arrow;
    public GameObject trackingArrow;
    public List<GameObject> enemiesList;
    // public GameObject shuriken;

    private Enemy _enemyScript;
    private GameObject _player;
    
    void Start()
    {
        _enemyScript = GetComponent<Enemy>();
        _player = GameObject.Find("Player");
        if (_player != null)
        {
            _playerFreezeTime = _player.GetComponent<PlayerControls>().GetIceFreezeTime();
        }
        StartCoroutine(attack());
    }

    IEnumerator attack()
    {
        while (true)
        {
            _enemyIsFrozen = _enemyScript.isFrozen;
            _attackInterval = _enemyIsFrozen? _attackInterval + _playerFreezeTime : 3f;
            int enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;
            
            // int randomAttack = Random.Range(0,3);
            int randomAttack = 0;
            
            switch (randomAttack)
            {
                case 0:
                    spawnArrows();
                    break;
                case 1:
                    spawnTrackingArrows(Random.Range(0, 2));
                    break;
                case 2:
                    if (enemyCount >= 4) break;
                    spawnEnemy();
                    break;
                case 3:
                    spawnShuriken();
                    break;
            }
            yield return new WaitForSeconds(_attackInterval);
        }
    }
        

    private void spawnArrows()
    {
        bool dir = Random.Range(0, 2) == 0;
        
        // > 0 high
        // -3 to 0 middle
        // < -3 low
        int level;
        if (_player.transform.position.y < -3)
        {
            level = 2;
        } 
        else if (_player.transform.position.y > 0)
        {
            level = 0;
        }
        else
        {
            level = 1;
        }
        
        int x = dir? -26 : 26;
        int[][] y = new int[3][];
        y[0] = new int[] {5, 2};
        y[1] = new int[] {-1, -3};
        y[2] = new int[] {-5, -6};
        
        for (int i = y[level][0]; i >= y[level][1]; i--)
        {
            arrow.GetComponent<Projectile>().moveDirection = x < 0? Vector2.right : Vector2.left;
            arrow.GetComponent<Projectile>().lifeTime = 5f;
            Instantiate(arrow, new Vector3(x, i, 0), Quaternion.identity);
        }
    }
    
    private void spawnTrackingArrows(int rand)
    {
        switch (rand)
        {
            case 0:
                Instantiate(trackingArrow, new Vector3(-21, 2, 0), Quaternion.identity);
                Instantiate(trackingArrow, new Vector3(-21, 0, 0), Quaternion.identity);
                Instantiate(trackingArrow, new Vector3(-21, -2, 0), Quaternion.identity);
                break;
            case 1:
                Instantiate(trackingArrow, new Vector3(21, 2, 0), Quaternion.Inverse(Quaternion.identity));
                Instantiate(trackingArrow, new Vector3(21, 0, 0), Quaternion.Inverse(Quaternion.identity) );
                Instantiate(trackingArrow, new Vector3(21, -2, 0), Quaternion.Inverse(Quaternion.identity));
                break;
        }
    }
    
    private void spawnEnemy()
    {
        for (int i = 0; i < 2; i++)
        {
            GameObject enemy = enemiesList[Random.Range(0, enemiesList.Count)];
            enemy.GetComponent<Enemy>().SetMaxHealth(1);
            enemy.GetComponent<Enemy>().isFoundPlayer = true;
            Instantiate(enemy, new Vector3(Random.Range(-20, 20), Random.Range(-5, 5), 0), Quaternion.identity);
        }
    }
    
    private void spawnShuriken()
    {
        
    }

}
