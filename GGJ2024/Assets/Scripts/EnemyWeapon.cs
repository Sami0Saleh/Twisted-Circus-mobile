using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EnemyWeapon : Weapon
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
           

            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (!player.isAirborne())
            {
                base.OnTriggerEnter2D(collision);
                player.TakeDamage(_damage, _knockback, Attacker.transform.position);
            }
            
        }
    }
}
