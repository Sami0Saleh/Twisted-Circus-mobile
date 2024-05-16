using UnityEngine;


public class PlayerFist : Weapon
{
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            EnemyController enemy = collision.gameObject.GetComponent<EnemyController>();
            base.OnTriggerEnter2D (collision);
            enemy.TakeDamage(_damage, _knockback);
            
                
        }

    }

    
}
