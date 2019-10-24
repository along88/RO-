using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaintainIconFacing : MonoBehaviour
{
    public Quaternion facing;
    // Start is called before the first frame update
    void Start()
    {
        facing = this.transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //maintain facing at all times here
        this.transform.rotation = facing;
    }
}
