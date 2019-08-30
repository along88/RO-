using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DelayTheme : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        var audioSource = GetComponent<AudioSource>();
        audioSource.PlayDelayed(.01f);
    }
}
