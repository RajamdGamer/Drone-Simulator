using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Movement : MonoBehaviour
{
    public Rigidbody rb;
    public int moveSpeed, maxSpeed;
    public float gravity;
    Vector3 movement;
    public ForceMode forcemode;
    public Transform cameraTransform;
    public float rotationSpeed = 10f;
    void Start()
    {
        
    }
    void FixedUpdate()
    {
        
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;
        
        if(rb.velocity.magnitude > maxSpeed)
        {
            rb.velocity = rb.velocity.normalized * maxSpeed;
        }
        if(Input.GetKey(KeyCode.Q))
        {
            rb.AddForce(Vector3.up * moveSpeed, forcemode);
        }
        if(Input.GetKey(KeyCode.E))
        {
            rb.AddForce(Vector3.down * moveSpeed, forcemode);
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            horizontalInput = 0;
            transform.Rotate(Vector3.up * rotationSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            horizontalInput = 0;
            transform.Rotate(Vector3.up * -rotationSpeed * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.Space))
        {
            Debug.Log("Shooting");
        }
        movement = (cameraForward * verticalInput + cameraRight * horizontalInput).normalized * moveSpeed;
        rb.AddForce(movement, forcemode);
        
    }
    // Update is called once per frame
    void Update()
    {   

    }

    void Gravity(){
        rb.AddForce(Vector3.down * gravity * Time.fixedDeltaTime);
    }
}
