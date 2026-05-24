using UnityEngine;
using System;

[RequireComponent(typeof(CharacterController))]
public class PlayerMotor : MonoBehaviour
{
    public float gravity = -20f;

    public bool IsGrounded { get; private set; }
    public float VerticalVelocity { get; private set; }
    public Vector3 CurrentHorizontal { get; private set; }

    public bool UseGravity { get; set; } = true;

    public event Action OnLanded;

    CharacterController controller;
    bool wasGrounded;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    public void Tick(float dt)
    {
        wasGrounded = IsGrounded;
        IsGrounded = controller.isGrounded;

        if (!wasGrounded && IsGrounded)
            OnLanded?.Invoke();

        if (IsGrounded && VerticalVelocity < 0f)
            VerticalVelocity = -2f;

        if (UseGravity)
            VerticalVelocity += gravity * dt;
    }

    public void SetVerticalVelocity(float value)
    {
        VerticalVelocity = value;
    }

    public void Move(Vector3 horizontal, float dt)
    {
        CurrentHorizontal = horizontal;

        Vector3 move = horizontal;
        move.y = VerticalVelocity;

        controller.Move(move * dt);
    }
}
