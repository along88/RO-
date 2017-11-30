using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MariePhysics  : Physics
{
    [SerializeField]
    private float lungeDistance = 5.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        Initialize(speed, fallMultipler);
        inputManager = GetComponent<InputManager>();
        player = GetComponent<Player>();
        wait = new WaitForSeconds(getUpDelay);
        defaultPosition = player.transform.eulerAngles;
        defaultSpeed = 20.0f;
    }

    private void Lunge()
    {
        if (player.IsGrounded)
        rb.position += (rb.transform.forward) * lungeDistance * Time.deltaTime;
        
    } //called during animation event
}

