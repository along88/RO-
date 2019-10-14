using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour, IActor
{
    private Vector3 movement;

    [SerializeField]
    private List<GameEvent> gameEvent;

    [SerializeField]
    private float moveSpeed;


	void Update ()
    {
        if (Input.GetButtonDown("Attack1"))
        {
            gameEvent[0].Raise();
        }

            
        Move(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));

        if (Input.GetButton("Dash2"))
        {
            gameEvent[1].Raise();
        }

    }

    public void Move(float xPos, float zPos)
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

    public void Attack()
    {
        
        gameObject.GetComponent<Animator>().SetTrigger("Attack");
        
    }

    public bool hasHypeAttack()
    {
        throw new System.NotImplementedException();
    }

    public void Hurt()
    {
        Debug.Log(name + ": OUCH!");

        //to DO
        /// ADD HURT LOGIC AND ANIMATIONS
    }
}
