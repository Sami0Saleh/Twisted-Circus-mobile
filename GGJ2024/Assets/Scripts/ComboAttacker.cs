using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ComboAttacker : MonoBehaviour
{
    [SerializeField] protected Weapon _weapon;
    public int _damage;
    public int _knockBack;
    public float attackDuration = 0.518f;
    public float betweenAttacksInterval;
    protected bool _canAttack = true;
    public float comboMultiplier;
    public float comboWindow;
    protected int _comboSize;
    protected float _comboCounter = 1;
    protected bool _isInCombo = false;

    protected MyTimer _comboTimer;
    protected MyTimer _betweenAttacksTimer;
    
    protected virtual void Start()
    {
        _comboTimer = new MyTimer(comboWindow);
        _betweenAttacksTimer = new MyTimer(betweenAttacksInterval);
    }

    protected virtual void Update()
    {
        { // Attack interval timing
            
            //Waiting for the attack interval to end
            if (_betweenAttacksTimer.IsOver())
            {
                _betweenAttacksTimer.Reset();
                _canAttack = true;
            }
            if (_canAttack)
            {
                Attack();
            }
            else
            {
                _betweenAttacksTimer.Tick();
            }
        }

        { // Combo timing
            if (_isInCombo && _canAttack)
            {
                _comboTimer.Tick();
                //Missed the window to chain combo
                if (_comboTimer.IsOver())
                {
                    _comboTimer.Reset();
                    ComboEnd();
                }
            }
        }
    }
    protected virtual void Attack()
    {
        if (_isInCombo)
        {
            _comboTimer.Reset();
        }
        _canAttack = false;

    }
    public void IncreaseCombo()
    {
        if (gameObject.tag == "Enemy")
        {
            Debug.Log("Combo Increased " + _comboCounter);
        }
        _isInCombo = true;
        _comboCounter++;
        if (_comboCounter > _comboSize)
        {
            ComboEnd();
        }
    }
    public void ComboEnd()
    {
        if(gameObject.tag == "Player")
        {
            Debug.Log("Combo Over");
        }
        
        _comboCounter = 1;
        _isInCombo = false;
    }
}
