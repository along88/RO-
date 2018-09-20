using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class Fighter : ScriptableObject
{
    [SerializeField]
    private string fighterName;

    [SerializeField]
    private float health;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float damage;

    public string FighterName { get { return fighterName; } set { fighterName = value; } }

    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }
    public float Damage
    {
        get { return damage; }
        set { damage = value; }
    }
}
