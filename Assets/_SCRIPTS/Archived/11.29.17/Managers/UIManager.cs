using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class UIManager : MonoBehaviour
{
    //I believe this class is never used, however it may be part of the Menu Screen scene
    private Image highlight;

    void Awake()
    {
        highlight = GameObject.FindGameObjectWithTag("Highlight").GetComponent<Image>();
    }

    public void Pressed()
    {
        Debug.Log("Clicked");
    }

   
}
