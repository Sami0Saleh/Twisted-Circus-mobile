using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGraphic : MonoBehaviour
{
    [SerializeField] Animator animator;
    public bool Movement;
    public bool Attack;
    public bool idle = true;

    private void Update()
    {
        if (Movement)
        {
            
        }  
    }
    public void StartMovementAnimation()
    {
        animator.Play("Walk");
    }
    public void StartAttack1Animation()
    {
        animator.Play("Attack1");
    }
    public void StartIdleAnimation()
    {
        animator.Play("Idle");
    }
}
