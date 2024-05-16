using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using static UnityEditor.Experimental.GraphView.GraphView;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[RequireComponent(typeof(BoxCollider2D))]
public class PlayerController : ComboAttacker
{
    [SerializeField] Animator animator;
    [SerializeField] LaughMeter laughMeter;
    [SerializeField] AudioSource PlayerAudioSource;
    [SerializeField] AudioSource MusicPlayer;
    [SerializeField] GameObject DeathScreen;
    [SerializeField] AudioClip DeathMusic;
    public enum Directions
    {
        North, South, West, East
    }
    public static float[] THRESHOLDS = { -1.89f, -3.48f, -7.91f, 7.58f };
    public static float safeSpace = 0.01f;

    [SerializeField] Transform playerTransform;
    [SerializeField] FloatingJoystick floatingJoystick;

    public float pushDistance;
    public float pushVelocity;
    public float speed = 4;
    public Vector3 jumpHeight = new Vector3(0, 2.5f, 0);
    public float jumpSpeed;
    public float fallSpeed;

    public float invunrabiltyInterval;
    private bool _isPushed = false;
    private float _pushDirection;
    private bool _isInvunrable = false;
    private MyTimer _invurnebltyT;
    private float _laughMeter;
    private bool _isFalling = false;
    private float _stunDur;
    private bool _isGrounded = true;
    private bool _isJumping = false;
    public float MeterPointsToDecrease;
    private Vector3 move;
    private bool _isFlipped = false;
    private Vector3 groundPos;
    private Vector3 jumpPos;
    private Vector3 gravity = new Vector3(0, -2.5f, 0);
    private Vector3 originalPosition;
    private bool _jumpCheck = false;
    private bool _attackCheck = false;

    public bool isAirborne()
    {
        return _isJumping || _isFalling;
    }
    protected override void Start()
    {
        base.Start();
        _comboSize = 3;
        _invurnebltyT = new MyTimer(invunrabiltyInterval);
    }
    protected override void Update()
    {
        if (_isPushed)
        {
            //Pushed left
            if (_pushDirection < 0)
            {
                if (transform.position.x >= THRESHOLDS[(int)Directions.West])
                {

                    if (!(transform.position.x - safeSpace < THRESHOLDS[(int)Directions.West]))
                    {
                        Pushed();
                    }
                    else
                    {
                        _isPushed = false;
                    }

                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.West], transform.position.y, transform.position.z);
                    _isPushed = false;

                }

            }
            //Pushed left
            else
            {
                if (transform.position.x <= THRESHOLDS[(int)Directions.East])
                {

                    if (!(transform.position.x + safeSpace > THRESHOLDS[(int)Directions.East]))
                    {
                        Pushed();
                    }
                    else
                    {
                        _isPushed = false;
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.East], transform.position.y, transform.position.z);
                    _isPushed = false;
                }
            }
        }
        else if (_isGrounded)
        {
            Move();

            if (_jumpCheck && !isAirborne())
            {
                originalPosition = Jump();
                _jumpCheck = false;
            }
            if (_isJumping)
            {
                animator.SetBool("Jumping", true);
                transform.Translate(jumpHeight * Time.deltaTime * jumpSpeed);

                if (transform.position.y >= originalPosition.y + jumpHeight.y)
                { _isJumping = false; _isFalling = true; }

            }
            if (_isFalling)
            {
                move = gravity;
                transform.Translate(move * Time.deltaTime * fallSpeed);

                if (transform.position.y <= originalPosition.y)
                { _isFalling = false;  animator.SetBool("Jumping", false); }
            }
        }
        else if (_isJumping)
        {
            transform.Translate(gravity * Time.deltaTime);

            if (jumpPos.y == originalPosition.y)
            {
                animator.SetBool("Jumping", false);
                _isGrounded = true;
                _isJumping = false;
            }
        }
        // Comabt calculations
        base.Update();

        if (_canAttack) { animator.SetBool("Attacking", false); }
        // Invulnerability timing
        if (_isInvunrable)
        {
            _invurnebltyT.Tick();

            if (_invurnebltyT.IsOver())
            {
                _invurnebltyT.Reset();
                _isInvunrable = false;
            }
        }
    }
    public void Pushed()
    {
        transform.Translate(new Vector3(_pushDirection, 0, 0) * pushVelocity * Time.deltaTime);
        if (Math.Abs(originalPosition.x - transform.position.x) >= pushDistance)
        {
            _isPushed = false;
        }
    }
    public void Move()
    {
        if (!isAirborne())
        {
            //down left
            if (floatingJoystick.Horizontal < 0 && floatingJoystick.Vertical < 0)
            {
                if (transform.position.y >= THRESHOLDS[(int)Directions.South])
                {

                    if (!(transform.position.y - safeSpace < THRESHOLDS[(int)Directions.South]))
                    {
                        transform.Translate(new Vector3(0, floatingJoystick.Vertical, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, THRESHOLDS[(int)Directions.South], transform.position.z);

                }
                _isFlipped = true;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                if (transform.position.x >= THRESHOLDS[(int)Directions.West])
                {

                    if (!(transform.position.x - safeSpace < THRESHOLDS[(int)Directions.West]))
                    {
                        transform.Translate(new Vector3(floatingJoystick.Horizontal, 0, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.West], transform.position.y, transform.position.z);

                }
            }
            //down right
            else if (floatingJoystick.Vertical < 0 && floatingJoystick.Horizontal > 0)
            {
                if (transform.position.y >= THRESHOLDS[(int)Directions.South])
                {

                    if (!(transform.position.y - safeSpace < THRESHOLDS[(int)Directions.South]))
                    {
                        transform.Translate(new Vector3(0, floatingJoystick.Vertical, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, THRESHOLDS[(int)Directions.South], transform.position.z);

                }
                if (transform.position.x <= THRESHOLDS[(int)Directions.East])
                {

                    if (!(transform.position.x + safeSpace > THRESHOLDS[(int)Directions.East]))
                    {
                        transform.Translate(new Vector3(floatingJoystick.Horizontal, 0, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.East], transform.position.y, transform.position.z);

                }
            }
            //up right
            else if (floatingJoystick.Vertical > 0 && floatingJoystick.Horizontal > 0)
            {
                if (transform.position.y <= THRESHOLDS[(int)Directions.North])
                {

                    if (!(transform.position.y + safeSpace > THRESHOLDS[(int)Directions.North]))
                    {
                        transform.Translate(new Vector3(0, floatingJoystick.Vertical, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, THRESHOLDS[(int)Directions.North], transform.position.z);

                }
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                if (transform.position.x <= THRESHOLDS[(int)Directions.East])
                {

                    if (!(transform.position.x + safeSpace > THRESHOLDS[(int)Directions.East]))
                    {
                        transform.Translate(new Vector3(floatingJoystick.Horizontal, 0, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.East], transform.position.y, transform.position.z);

                }
            }
            //up left
            else if (floatingJoystick.Vertical > 0 && floatingJoystick.Horizontal < 0)
            {
                if (transform.position.y <= THRESHOLDS[(int)Directions.North])
                {

                    if (!(transform.position.y + safeSpace > THRESHOLDS[(int)Directions.North]))
                    {
                        transform.Translate(new Vector3(0, floatingJoystick.Vertical, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, THRESHOLDS[(int)Directions.North], transform.position.z);

                }
                _isFlipped = true;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                if (transform.position.x >= THRESHOLDS[(int)Directions.West])
                {

                    if (!(transform.position.x - safeSpace < THRESHOLDS[(int)Directions.West]))
                    {
                        transform.Translate(new Vector3(floatingJoystick.Horizontal, 0, 0) * speed * 0.8f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.West], transform.position.y, transform.position.z);

                }
            }
            //Only down
            else if (floatingJoystick.Vertical < 0)
            {
                if (transform.position.y >= THRESHOLDS[(int)Directions.South])
                {

                    if (!(transform.position.y - safeSpace < THRESHOLDS[(int)Directions.South]))
                    {
                        transform.Translate(new Vector3(0, floatingJoystick.Vertical, 0) * speed * 1f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, THRESHOLDS[(int)Directions.South], transform.position.z);

                }
            }
            //Only up
            else if (floatingJoystick.Vertical > 0)
            {
                if (transform.position.y <= THRESHOLDS[(int)Directions.North])
                {

                    if (!(transform.position.y + safeSpace > THRESHOLDS[(int)Directions.North]))
                    {
                        transform.Translate(new Vector3(0, floatingJoystick.Vertical, 0) * speed * 1f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(transform.position.x, THRESHOLDS[(int)Directions.North], transform.position.z);

                }
            }
            //Only left
            else if (floatingJoystick.Horizontal < 0)
            {
                _isFlipped = true;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                if (transform.position.x >= THRESHOLDS[(int)Directions.West])
                {

                    if (!(transform.position.x - safeSpace < THRESHOLDS[(int)Directions.West]))
                    {
                        transform.Translate(new Vector3(floatingJoystick.Horizontal, 0, 0) * speed * 1f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.West], transform.position.y, transform.position.z);

                }
            }
            //Only right
            else if (floatingJoystick.Horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                if (transform.position.x <= THRESHOLDS[(int)Directions.East])
                {

                    if (!(transform.position.x + safeSpace > THRESHOLDS[(int)Directions.East]))
                    {
                        transform.Translate(new Vector3(floatingJoystick.Horizontal, 0, 0) * speed * 1f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.East], transform.position.y, transform.position.z);

                }
            }
            else animator.SetBool("Walking", false);
        }
        else
        {
            // only left
            if (floatingJoystick.Horizontal < 0)
            {
                _isFlipped = true;
                transform.rotation = Quaternion.Euler(new Vector3(0f, 180f, 0f));
                if (transform.position.x >= THRESHOLDS[(int)Directions.West])
                {

                    if (!(transform.position.x - safeSpace < THRESHOLDS[(int)Directions.West]))
                    {
                        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * 1f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.West], transform.position.y, transform.position.z);

                }
            }
            // only right
            else if (floatingJoystick.Horizontal > 0)
            {
                transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, 0f));
                if (transform.position.x <= THRESHOLDS[(int)Directions.East])
                {

                    if (!(transform.position.x + safeSpace > THRESHOLDS[(int)Directions.East]))
                    {
                        transform.Translate(new Vector3(Input.GetAxis("Horizontal"), 0, 0) * speed * 1f * Time.deltaTime, Space.World);
                        animator.SetBool("Walking", true);
                    }
                }
                else
                {
                    transform.position = new Vector3(THRESHOLDS[(int)Directions.East], transform.position.y, transform.position.z);

                }
            }
            move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
        }
    }
    public void JumpCheck()
    {
        _jumpCheck = true;
    }
    public void AttackCheck()
    {
        _attackCheck = true;
    }
    public Vector3 Jump()
    {
        originalPosition = transform.position;
        _isJumping = true;
        return originalPosition;
    }
    protected override void Attack()
    {

        if (_attackCheck && !isAirborne())
        {
            base.Attack();
            animator.SetBool("Attacking", true);
            PlayerAudioSource.Play();
            switch (_comboCounter)
            {
                case 1:
                    _weapon.Attack(_damage, 0);
                    break;
                case 2:
                    _weapon.Attack(_damage * comboMultiplier, 0);
                    break;
                case 3:
                    _weapon.Attack(_damage * comboMultiplier * comboMultiplier, _knockBack);
                    break;
                default:
                    Debug.Log("I fucked up");
                    break;
            }
        }
        else if (_attackCheck)
        {
            Debug.Log("Airborne attack");
        }
        _attackCheck = false;
    }
    public void TakeDamage(float damage, float knockback, Vector3 enemyPosition)
    {

        if (!_isInvunrable)
        {
            //Things that happen when not invulnerable

            //knockback handling
            if (knockback != 0)
            {
                Knockback(knockback, enemyPosition);
            }

            //laugth meter handling
            laughMeter.loseLaugh(MeterPointsToDecrease);

        }
    }
    private void Knockback(float knockback, Vector3 enemyPosition)
    {
        _isPushed = true;
        _isInvunrable = true;
        originalPosition = transform.position;
        
        _pushDirection = (-(enemyPosition.x - transform.position.x)) / Mathf.Abs(enemyPosition.x - transform.position.x);
        
    }
    public void BoostPlayerDamage()
    {
        _damage++;
        EnemyController.currentHp += 10;
    }
    public void BoostPlayerSpeed()
    {
        speed += 0.2f;
        EnemyController.movementSpeed += 0.2f;
    }
    public void BoostPlayerCrit()
    {
        comboMultiplier++;
        EnemyController.currentHp += 10;
    }
    public void GameEnd()
    {
        MusicPlayer.clip = DeathMusic;
        MusicPlayer.Play();
        DeathScreen.SetActive(true);
        //animator.SetBool("Death", true);
        Time.timeScale = 0;
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}