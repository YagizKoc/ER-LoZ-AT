using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorDriver : MonoBehaviour
{
    Animator anim;
    InputBuffer inputBuffer;
    Attack attack;
    PlayerController controller;

    void Awake()
    {
        anim = GetComponent<Animator>();
        inputBuffer = GetComponent<InputBuffer>();
        attack = GetComponent<Attack>();
        controller = GetComponent<PlayerController>();
    }

    public void TriggerAttack()
    {
        anim.SetTrigger("Attack");
    }

    public void TriggerDodge()
    {
        anim.SetTrigger("Dodge");
    }

    public void SetComboIndex(int comboIndex)
    {
        anim.SetInteger("ComboIndex", comboIndex);
    }

    public void SetLockOn(bool lockedOn) => anim.SetBool("IsLockedOn", lockedOn);
    public void SetIsMoving(bool isMoving) => anim.SetBool("IsMoving", isMoving);
    public void SetMove(Vector2 input)
    {
        anim.SetFloat("MoveX", input.x);
        anim.SetFloat("MoveY", input.y);
    }
    public void SetSpeed(float speed01) => anim.SetFloat("Speed", speed01, 0.15f, Time.deltaTime);
    public void SetGrounded(bool grounded) => anim.SetBool("IsGrounded", grounded);
    public void SetYVel(float yVel) => anim.SetFloat("YVel", yVel);

    public void SetDoAttack2(bool v) => anim.SetBool("Attack2", v);
    public void SetDoAttack3(bool v) => anim.SetBool("Attack3", v);

    public void OnAttackEnd() 
    {
        Debug.Log("OnAttackEndiscalled");
        controller.comboIndex = 0;
    }

    public void EndAttack1()
    {
        if (controller.comboIndex == 0) 
        {
            attack.EndAttack();
        }
        attack.SwingStart();
    }

    public void EndAttack2()
    {
        Debug.Log("EndAttack2 çağrıldı");
        if (controller.comboIndex == 1)
        {
            Debug.Log("EndAttack2 çağrıldı ve if geldi");
            attack.EndAttack();
        }
        attack.SwingStart();
    }

    public void EndAttack3()
    {
        
           attack.EndAttack();

    }
}