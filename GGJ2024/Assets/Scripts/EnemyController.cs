using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting;
using UnityEngine;
using static PlayerController;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : ComboAttacker
{
    [SerializeField] private Animator enemyAnimator;
    private PlayerController _player;
    public static float currentHp = 50;
    public static float movementSpeed = 1.2f;
    public float MeterPointToAdd;
    public bool enemyIsDead = false;
    public float rotationSpeed = 5f;
    public float attackModeThreshold = 1f;
    private LaughMeter _laughMeter;

    private bool _isPrefabFlipped;
    private bool _isLookingLeft = true;
    private Vector3 direction;

    protected float distance; // Tomer: I need it as a field to check when to attack
    protected override void Start()
    {
        base.Start();
        _comboSize = 2;
        _isPrefabFlipped = transform.rotation.y == 1;
    }
    protected override void Update()
    {
        distance = Vector3.Distance(_player.transform.position, transform.position);

        base.Update();

        direction = _player.transform.position - transform.position;
        direction.Normalize();

            WhereToLookAt();
            if (distance > attackModeThreshold)
            {
                direction = _player.transform.position - transform.position;
                direction.Normalize();

                if (transform.position.y < PlayerController.THRESHOLDS[(int)PlayerController.Directions.North])
                {
                    //Good Y space
                    if (!(transform.position.y + safeSpace * 5 > THRESHOLDS[(int)PlayerController.Directions.North]))
                    {
                        // Will not go over
                        NormalMovingBehaviour();
                    }
                    else
                    {   // Will go over
                        EdgeMovmentBehaviour();
                    }
                }
                else
                {
                    EdgeMovmentBehaviour();
                }
            }
            else enemyAnimator.SetBool("Walking", false);

        if (_canAttack)
        {
            enemyAnimator.SetBool("Attack", false);
        }
    }
        private void WhereToLookAt()
        {
        if (_isPrefabFlipped)
        {
            if (direction.x < 0)
            {

                //look left
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));

            }
            else
            {
                //look right
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
            }
        }
        else
        {
            if (direction.x < 0)
            {

                //look left
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));

            }
            else
            {
                //look right
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
            }
        }
    }
        private void NormalMovingBehaviour()
        {
        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Walking", true);
        transform.Translate(movementSpeed * Time.deltaTime * direction, Space.World);

            //The teleport for small distance in the Y axis
            /* if (Mathf.Abs(playerTransform.position.y - transform.position.y) < 0.5)
             {
                 transform.position = new Vector3(transform.position.x, playerTransform.position.y, transform.position.z);
             }*/
        }
        private void EdgeMovmentBehaviour()
        {
        enemyAnimator.SetBool("Attack", false);
        enemyAnimator.SetBool("Walking", true);
        transform.Translate(new Vector3(direction.x, -1, 0) * movementSpeed * Time.deltaTime, Space.World);
        }
        public void SetPlayer(PlayerController player)
        {
            _player = player;
        }
        public void SetLaughMeter(LaughMeter laughMeter)
        {
            _laughMeter = laughMeter;
        }
    
    protected override void Attack()
    {
        if (distance <= attackModeThreshold)
        {
            enemyAnimator.SetBool("Attack", true);
            base.Attack();
            switch (_comboCounter)
            {
                case 1:
                    _weapon.Attack(_damage, 0);
                    
                    //Debug.Log("First hit");
                    break;
                case 2:
                    _weapon.Attack(_damage * comboMultiplier, _knockBack);
                    //Debug.Log("Second hit");
                    break;
                default:
                    Debug.Log("I fucked up");
                    break;
            }
        }
        
    }
    public void TakeDamage(float damage, float knockback)
    {
        currentHp -= damage;
        if (currentHp <= 0)
        {
            Die();
        }
        else if (knockback != 0)
        {
            Knockback(knockback);
        }
    }
    private void Knockback(float knockback) //need to check if it needs to change 
    {
        Debug.Log("Enemy Knockback");
        Vector3 newPosition = transform.position;
        float knockDir = (-(_player.transform.position.x - newPosition.x)) / Mathf.Abs(_player.transform.position.x - newPosition.x);
        newPosition.x = transform.position.x + knockback * knockDir;
        transform.position = newPosition;
    }
    private void Die()
    {
        /*animator.ResetTrigger("Walk1");
        FallAnimation();*/
        _laughMeter.gainLaugh(MeterPointToAdd); //Laugth meter handling
        Spawner.enemyCount--;
        Destroy(gameObject);
    }



}