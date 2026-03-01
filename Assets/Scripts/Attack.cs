using UnityEngine;
using System;

public class Attack : MonoBehaviour
{
    CharacterStats stats;
    Dodge dodge;
    PlayerMotor playerMotor;
    InputBuffer inputBuffer;
    PlayerAnimatorDriver playerAnimatorDriver;
    PlayerController controller;

    public Collider hitboxCollider;

    HitboxDamage hitboxDamage;   // <<< EKLE

    public bool IsAttacking { get; private set; }

    public event Action OnAttackStart;
    public event Action OnAttackEnd;

    AttackData current;
    float attackTimer;
    float elapsed;

    bool canAttack = true;
    float cooldownTimer;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        dodge = GetComponent<Dodge>();
        playerMotor = GetComponent<PlayerMotor>();
        inputBuffer = GetComponent<InputBuffer>();
        playerAnimatorDriver = GetComponent<PlayerAnimatorDriver>();

        if (hitboxCollider)
        {
            hitboxCollider.enabled = false;
            hitboxDamage = hitboxCollider.GetComponent<HitboxDamage>(); // <<< EKLE
        }
    }

    public void Tick(float dt)
    {
        if (IsAttacking && current != null)
        {
            attackTimer -= dt;
            elapsed += dt;

            bool hitOn = elapsed >= current.hitboxOnTime && elapsed <= current.hitboxOffTime;
            if (hitboxCollider) hitboxCollider.enabled = hitOn;

            /*if (attackTimer <= 0f)
                EndAttack();*/ //combo için buraları akldırdın
        }

        if (!canAttack)
        {
            cooldownTimer -= dt;
            if (cooldownTimer <= 0f)
                canAttack = true;
        }
    }

    public bool TryAttack(AttackData data)
    {
        if (data == null) return false;
        if (!canAttack || IsAttacking || dodge.IsDodging || !playerMotor.IsGrounded)
            return false;

        current = data;
        IsAttacking = true;

        elapsed = 0f;
        attackTimer = data.totalTime;

        canAttack = false;
        cooldownTimer = data.totalTime;

        if (hitboxDamage)
        {
            hitboxDamage.attacker = transform; 
            hitboxDamage.ResetHits();         
        }

        if (hitboxCollider) hitboxCollider.enabled = false;

        OnAttackStart?.Invoke();
        return true;
    }

    public void EndAttack()
    {
        Debug.Log("EndAttack geldi");
 
            IsAttacking = false;
            if (hitboxCollider) hitboxCollider.enabled = false;
            current = null;
            OnAttackEnd?.Invoke();
        
    }

    public void SwingStart()
    {
        elapsed = 0f;              
        if (hitboxDamage) hitboxDamage.ResetHits();
    }

    public void SwingEnd()
    {
        if (hitboxCollider) hitboxCollider.enabled = false;
    }

}
