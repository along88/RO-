using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{


    [SerializeField]
    public GameObject[] firePoint = new GameObject[2];
    public List<GameObject> VFX;
    [SerializeField]
    private GameObject effectToSpawn;
    private void Awake()
    {
        
        
        effectToSpawn = VFX[0];
       // SpawnFX();
        //OnAwake Instantiate A projectile Object


    }

    public void SpawnFX(int firePointID)
    {
        GameObject fx;

        if(firePoint != null)
        {
            effectToSpawn.GetComponent<projectileHitBox>().player = this.GetComponent<Player>();
            fx = Instantiate(effectToSpawn, firePoint[firePointID].transform);
           


        }
    }
}
