using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerMotor))]
[RequireComponent(typeof(Dodge))]
[RequireComponent(typeof(Attack))]
[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(LockOn))]
[RequireComponent(typeof(PlayerAnimatorDriver))]
public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float turnSpeed = 12f;
    public Transform cameraTransform;
    public float jumpForce = 8f;
    public AttackData attackData;
    Quaternion dodgeTargetRot;
    bool rotateToDodge;
    public int comboIndex;

    [Header("Rotation Lock (prevents jitter)")]
    public float lockOnRotationHoldAfterDodge = 0.15f; // 0.12-0.20 iyi
    float lockOnRotationHoldTimer = 0f;

    PlayerMotor motor;
    Dodge dodge;
    Attack attack;
    CharacterStats stats;
    LockOn lockOn;
    PlayerAnimatorDriver animDriver;
    InputBuffer inputBuffer;

    Vector2 moveInput;
    const float MOVE_EPS_SQR = 0.0004f;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        motor = GetComponent<PlayerMotor>();
        dodge = GetComponent<Dodge>();
        attack = GetComponent<Attack>();
        lockOn = GetComponent<LockOn>();
        animDriver = GetComponent<PlayerAnimatorDriver>();
        inputBuffer = GetComponent<InputBuffer>();

    }

    void Update()
    {
        float dt = Time.deltaTime;

        if (lockOnRotationHoldTimer > 0f)
            lockOnRotationHoldTimer -= dt;

        stats.Tick(dt);
        motor.Tick(dt);
        dodge.Tick(dt);
        attack.Tick(dt);

        if (!dodge.IsDodging) rotateToDodge = false;

        if (attack.IsAttacking)
        {
            motor.Move(Vector3.zero, dt);
            return;
        }


        if (!attack.IsAttacking)
        {
            comboIndex = 0;
        }

        /*if (comboIndex == 0) 
        {
            animDriver.SetDoAttack2(false);
            animDriver.SetDoAttack3(false);
        }*/

        if (moveInput.sqrMagnitude < 0.25f * 0.25f) //Analog Dead Zone
            moveInput = Vector2.zero;
        Vector3 moveDir = GetCameraRelativeMove(moveInput);

        Vector3 faceDir = lockOn.IsLockedOn
            ? lockOn.GetTargetFlatDirection(transform.position)
            : moveDir;

        bool allowLockOnFacing = !dodge.IsDodging && lockOnRotationHoldTimer <= 0f;

        if (allowLockOnFacing && faceDir.sqrMagnitude > 0.0001f)
        {
            Quaternion targetRot = Quaternion.LookRotation(faceDir, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, turnSpeed * dt);
        }

        float lockMult = lockOn.IsLockedOn ? 0.80f : 1f;   // 0.8 = lock-on’da %20 daha yavaş
        Vector3 horizontal = dodge.GetHorizontalVelocity(moveDir, speed * lockMult);
        // lock-on'da çok küçük hareketleri iptal et
        if (lockOn.IsLockedOn && horizontal.magnitude < 0.75f)
        {
            horizontal = Vector3.zero;
        }

        if (rotateToDodge)
        {
            transform.rotation = Quaternion.Slerp(
            transform.rotation,
            dodgeTargetRot,
            turnSpeed * Time.deltaTime
            );


            if (Quaternion.Angle(transform.rotation, dodgeTargetRot) < 1f)
                rotateToDodge = false;
        }

        motor.Move(horizontal, dt);

        // === ANIM ===
        if (animDriver != null)
        {
            float speed01 = Mathf.Clamp01(horizontal.magnitude / speed);
            bool isMoving = speed01 > 0.15f;

            animDriver.SetLockOn(lockOn.IsLockedOn);
            animDriver.SetIsMoving(isMoving);

            Vector2 animInput;

            if (lockOn.IsLockedOn)
            {
                // moveDir world -> karakter local (x: strafe, z: forward/back)
                Vector3 local = transform.InverseTransformDirection(moveDir);
                animInput = new Vector2(local.x, local.z);
            }
            else
            {
                animInput = moveInput;
            }

            if (animInput.sqrMagnitude > 1f) animInput.Normalize();
            animDriver.SetMove(animInput);


            animDriver.SetSpeed(speed01); // <<< ÖNEMLİ
            animDriver.SetGrounded(motor.IsGrounded);
            animDriver.SetYVel(motor.VerticalVelocity);
            float yVel = motor.VerticalVelocity;
            if (motor.IsGrounded) yVel = 0f;   // <<< ÖNEMLİ
            animDriver.SetYVel(yVel);
        }
    }

    Vector3 GetCameraRelativeMove(Vector2 input)
    {
        if (!cameraTransform)
            return new Vector3(input.x, 0f, input.y);

        Vector3 f = cameraTransform.forward; f.y = 0f; f.Normalize();
        Vector3 r = cameraTransform.right; r.y = 0f; r.Normalize();

        Vector3 dir = r * input.x + f * input.y;
        if (dir.sqrMagnitude > 1f) dir.Normalize();
        return dir;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }



    public void OnJump(InputValue value)
    {
        if (value.Get<float>() < 0.5f) return;
        if (dodge.IsDodging || attack.IsAttacking) return;
        if (motor.IsGrounded) motor.SetVerticalVelocity(jumpForce);
    }

    public void OnDodge(InputValue value)
    {
        if (value.Get<float>() < 0.5f) return;
        if (!motor.IsGrounded || attack.IsAttacking || !cameraTransform) return;

        Vector3 dir = GetCameraRelativeMove(moveInput);
        if (!dodge.TryStart(dir)) return;
        lockOnRotationHoldTimer = lockOnRotationHoldAfterDodge;

        animDriver.TriggerDodge();

        Vector3 flat = dir; flat.y = 0f;
        if (flat.sqrMagnitude > 0.0001f)
        {
            dodgeTargetRot = Quaternion.LookRotation(flat, Vector3.up);
            rotateToDodge = true;
        }
    }

    public void OnAttack(InputValue value)
{
    if (value.Get<float>() < 0.5f) return;
    if (dodge.IsDodging) return;

    if (attack.IsAttacking)
    {
        comboIndex++;

            if (comboIndex >= 1) 
            {
                animDriver.SetDoAttack2(true);
            }
            if (comboIndex >= 2) 
            {
                animDriver.SetDoAttack3(true);
            }
            

        return;
    }

    comboIndex = 0;
    animDriver.SetDoAttack2(false);
    animDriver.SetDoAttack3(false);

    attack.TryAttack(attackData);
}

    public void OnLockOn(InputValue value)
    {
        if (value.Get<float>() < 0.5f) return;
        lockOn.ToggleLock(transform);
    }

    void OnEnable()
    {
        attack.OnAttackStart += animDriver.TriggerAttack;
    }
    void OnDisable()
    {
        attack.OnAttackStart -= animDriver.TriggerAttack;
    }

}
