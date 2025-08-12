using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    
    public Rigidbody rb;
    int health = 100;
    public bool mayday = false;
    public float mayDayRotationSpeed = 300f, mayDayDownForce = 50f;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    public void SetHealth()
    {
        if (health <= 0)
        {
            mayday = true;
        }
        health -= 20;
    }

    void Update(){
        if (mayday == true)
        {
            MayDay();
        }
    }

    private void MayDay()
    {
        transform.Rotate(Vector3.up * mayDayRotationSpeed * Time.deltaTime);
        rb.AddForce(Vector3.down * 100f, ForceMode.Acceleration);
        Debug.Log("MayDay! MayDay!");
    }

}
