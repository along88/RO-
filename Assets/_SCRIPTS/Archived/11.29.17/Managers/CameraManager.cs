using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


class CameraManager : MonoBehaviour
{
    private object[] cameras;

    private void OnServerInitialized()
    {
        //get cameras

        cameras = GameObject.FindGameObjectsWithTag("Cameras");

    }
}

