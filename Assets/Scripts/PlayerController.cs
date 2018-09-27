using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Vector3 movement;

    [SerializeField]
    private GameEvent gameEvent;

    void Awake()
    {
        
    }

    void Start ()
    {
        
	}

	void Update ()
    {
        Move(Input.GetAxis("Horizontal2"), Input.GetAxis("Vertical2"));

        if (Input.GetMouseButtonDown(0))
        {
            gameEvent.Raise();
        }


    }

    private void Move(float xPos, float zPos)
    {
        movement = new Vector3(xPos, 0.0f, zPos);
        transform.Translate(movement * 20.0f * Time.deltaTime, Space.World);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 20.0f);
    }

    public void OnHit()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.blue;
    }

   


}
