using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOrbit : MonoBehaviour {
    [SerializeField]
    private GameObject stage;
    // Use this for initialization

      
    

        void FixedUpdate()
        {
        transform.RotateAround(stage.transform.position, Vector3.down, 20 * Time.deltaTime);
        }
    
}
