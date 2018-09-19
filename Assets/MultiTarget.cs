using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiTarget : MonoBehaviour {

    [SerializeField]
    private List<Transform> targets;

    private Vector3 centerPoint;

    [SerializeField]
    private Vector3 offSet;

    private Camera cam;
    [SerializeField]
    private float minZoom; //40f
    [SerializeField]
    private float maxZoom; //10.0f
    [SerializeField]
    private float zoomLimiter; //50f






    // Use this for initialization
    void Start () {
        cam = GetComponent<Camera>();
        
	}
	
	// Update is called once per frame
	void LateUpdate () {
        centerPoint = GetCenterPoint();
        Vector3 newPostion = centerPoint + offSet;
        transform.position = newPostion;
        CameraZoom();

    }

    private Vector3 GetCenterPoint()
    {

        if(targets.Count == 1)
                return targets[0].position;

        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
            bounds.Encapsulate(targets[i].position);
        
        return bounds.center;

    }

    private void CameraZoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetGreatestDistance() / zoomLimiter);
        cam.fieldOfView = newZoom;
    }

    private float GetGreatestDistance()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);

        for (int i = 0; i < targets.Count; i++)
            bounds.Encapsulate(targets[i].position);

        return bounds.size.x;
    }
}
