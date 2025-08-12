using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour
{
   
    Transform closestEnemy;
    public Transform initialTransform;
    public int missileSpeed, bulletSpeed;
    void Start()
    {
        initialTransform = transform;
    }

    void OnEnable()
    {
        
        initialTransform = transform;
    }

    void FixedUpdate()
    {
        closestEnemy = FindClosestEnemy();
            if (closestEnemy != null)
            {
                    transform.LookAt(closestEnemy.transform);
                    transform.position += (closestEnemy.transform.position - transform.position).normalized * missileSpeed * Time.fixedDeltaTime;
                    if (Vector3.Distance(transform.position, closestEnemy.transform.position) < 0.3f)
                    {
                        if (closestEnemy.GetComponent<Enemy>().enemyHealth <= 0)
                        {
                            closestEnemy.GetComponent<Enemy>().Die();
                        }
                        else
                        {
                            closestEnemy.GetComponent<Enemy>().enemyHealth -= 1;
                        }
                        ObjectPool.Instance.SpawnFromPool("ExplosionParticle", gameObject.transform.position, gameObject.transform.rotation);
                        ObjectPool.Instance.ReturnToPool(gameObject);
                    }
            }
            else
            {
                transform.position += transform.forward * missileSpeed * Time.fixedDeltaTime;
            }
    }
        
    

    Transform FindClosestEnemy() {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform closestEnemy = null;
        float closestDistanceSqr = Mathf.Infinity; 

        foreach (GameObject enemy in enemies)
        {
            Vector3 directionToEnemy = enemy.transform.position - initialTransform.position;
            float dSqrToEnemy = directionToEnemy.sqrMagnitude; 
            if (dSqrToEnemy < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToEnemy;
                closestEnemy = enemy.transform;
            }
        }
        return closestEnemy;
    }
    // void OnCollisionEnter(Collision other)
    // {
        
    //     ObjectPool.Instance.ReturnToPool(gameObject);
    // }
}
