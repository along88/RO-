using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 movement;

    [SerializeField]
    private GameEvent gameEvent;

    [SerializeField]
    private float moveSpeed;


    void Awake()
    {
        
    }

    void Start ()
    {
        
	}

	void Update ()
    {
        Move(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));

        if (Input.GetButton("Dash2"))
        {
            gameEvent.Raise();
        }

    }

    private void Move(float xPos, float zPos)
    {
        movement = new Vector3(xPos, 0.0f, zPos);
        transform.Translate(movement.normalized * moveSpeed * Time.deltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), moveSpeed);
        }
        
    }

    public void Dash()
    {
        moveSpeed += 2.0f;
    }



   


}
