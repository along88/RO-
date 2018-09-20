using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField]
    private Fighter fighter;

    private Rigidbody rb;
    private Quaternion newRotation;

    public  Fighter Fighter
    {
        get { return fighter; }
        set { fighter = value; }
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        newRotation = new Quaternion(0.0f, -135.0f, 0.0f, 0.0f);
    }

    private void Update()
    {
        Move(Input.GetAxisRaw("Horizontal1"), Input.GetAxisRaw("Vertical1"));
       
    }

    private void Move(float x, float z)
    {
        Vector3 moveDirection = new Vector3(x * Fighter.Speed, 0.0f, z * Fighter.Speed) * Time.deltaTime;

        rb.velocity = moveDirection;

        Rotate(moveDirection);
       
        
    }
    private void Rotate(Vector3 direction)
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(direction),5.0f * Time.deltaTime);

    }
}