using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    public float nextLaunch, fireRate;
    public Transform bulletSpawner;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKey(KeyCode.Space) && Time.time >= nextLaunch)
        {
            nextLaunch = Time.time + fireRate;
            ObjectPool.Instance.SpawnFromPool("Missile", bulletSpawner.position, bulletSpawner.rotation);
        }
    }
}
