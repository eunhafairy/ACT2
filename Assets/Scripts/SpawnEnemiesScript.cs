using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemiesScript : MonoBehaviour
{
    [SerializeField] GameObject enemy, fly_enemy;
    int chance = 0;
    float halfScreenSizeX;
    public bool flag;
    void Awake()
    {
      

         halfScreenSizeX = Camera.main.aspect * Camera.main.orthographicSize;
    }
    void SpawnEnemies() {
        
        chance = Random.Range(1, 10);
            Vector2 randomPos = new Vector2(Random.Range(-halfScreenSizeX + 7, halfScreenSizeX - 7), Random.Range(-4, 4));
            if (chance <= 5)
            {

                Instantiate(enemy, randomPos, Quaternion.identity);
            }
            else
            {
                Instantiate(fly_enemy, randomPos, Quaternion.identity);
            }
        }
    public void invokeFunction() {
        InvokeRepeating("SpawnEnemies", 10, 10);
    }
     
         
        
    }


