using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageType : MonoBehaviour {

    [SerializeField]
    private DmgType damageType;

    [SerializeField]
    private float minDamage;

    [SerializeField]
    private float maxDamage;

    [SerializeField]
    private float currentDamage;

    public float MinDamage { get { return minDamage; }}
    public float MaxDamage { get { return maxDamage; }}

    // Use this for initialization
    void Start ()
    {
        Initialize();
    }
	
    private void Initialize()
    {
        switch (damageType)
        {
            case DmgType.LIGHT:
                //set damage 
                minDamage = 5.0f;
                maxDamage = 10.0f;
                currentDamage = CurrentDamage(minDamage, maxDamage);
               break;
            case DmgType.MEDIUM:
                minDamage = 10.0f;
                maxDamage = 15.0f;
                //currentDamage = CurrentDamage(minDamage, maxDamage);
                break;
            case DmgType.HEAVY:
                minDamage = 15.0f;
                maxDamage = 20.0f;
                //currentDamage = CurrentDamage(minDamage, maxDamage);
                break;
            default:
                Debug.LogError("Please Select a Damage Type");
                break;
        }
    }

    public float CurrentDamage(float minDmg, float maxDmg)
    {
        float dmg = Random.Range(minDamage,maxDmg);
        //Debug.Log("Dmg Output:"+currentDamage.ToString());
        return dmg;
    }
}
public enum DmgType
{
    LIGHT,
    MEDIUM,
    HEAVY,
    NONE
}
