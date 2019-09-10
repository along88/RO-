using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    [SerializeField]
    private BoxCollider hypeAttackHitBox;
    [SerializeField]
    private BoxCollider attackHitBox;
    [SerializeField]
    private BoxCollider knockBackHitBox;
    [SerializeField]
    private BoxCollider torso;
    [SerializeField]
    private BoxCollider blockArea;
    private BoxCollider ground;
    //Player Reference
    private Player player;
    //delay between active attacks

    [SerializeField]
    private float delaySeconds;
    private WaitForSeconds delay;

    [SerializeField]
    private GameObject AttackVFX;


    public BoxCollider Torso
    {
        get {return torso; }
        set {torso = value; }
    }

    private void Awake()
    {
        delay = new WaitForSeconds(delaySeconds);
        player = GetComponentInParent<Player>();
    }
    private void Start()
    {
        
        torso.name += player.ID.ToString();
        blockArea.name += player.ID.ToString();
    }
    private void LateUpdate()
    {
        CloseBlockArea();
    }

    #region HitBoxLogic
    private void OpenHitBox()
    {
       
        if (player.IsGrounded)
            attackHitBox.enabled = true;
        else
            attackHitBox.enabled = true;
        StartCoroutine("CloseHitBoxes"); // this disables the hitbox incase the animation event did not


        
    }
    private void CloseHitBox()
    {
        

        attackHitBox.enabled = false;
        hypeAttackHitBox.enabled = false;
        knockBackHitBox.enabled = false;
        player.CanMove = true;
    }
    private void OpenBlockArea()
    {
        blockArea.enabled = true;
        player.CanMove = false;
    }
    private void CloseBlockArea()
    {
        if(!player.IsDefending)
        blockArea.enabled = false;
        player.CanMove = true;
    }
    private void OpenHypeHitBox()
    {
        if (player.IsGrounded)
        {
            hypeAttackHitBox.enabled = true;
            
            StartCoroutine("CloseHype"); // this disables the hitbox incase the animation event did not

        }
    }
    private void OpenKnockBackHitBox()
    {

        if (player.IsGrounded)
        {

            knockBackHitBox.enabled = true;
            //player.AttackCounter++;
        }
            StartCoroutine("CloseHitBoxes"); // this disables the hitbox incase the animation event did not

    }

    private IEnumerator CloseHype()
    {
        WaitForSeconds delayHype = new WaitForSeconds(0.5f);
        yield return delayHype;
        player.IsHyped = false;
        CloseHitBox();
    }
    private IEnumerator CloseHitBoxes()
    {
        yield return delay;
        CloseHitBox();
    }
    #endregion

    #region ATTACKVFX
    public void EnableVFX()
    {
        AttackVFX.active = true;
        if (AttackVFX.GetComponent<ProjectileSpawner>())
        {
            var projectile = AttackVFX.GetComponent<ProjectileSpawner>();
            projectile.enabled = true;
        }
    }
    public void DisableVFX()
    {
        AttackVFX.active = false;
        if (AttackVFX.GetComponent<ProjectileSpawner>())
        {
            var projectile = AttackVFX.GetComponent<ProjectileSpawner>();
            projectile.enabled = false;
        }
    }
    #endregion
}
