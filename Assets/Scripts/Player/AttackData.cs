using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Attack Data")]
public class AttackData : ScriptableObject
{
    [Header("Animation")]
    public string animStateName = "Attack_1";   // Animator state adı
    public float crossFade = 0.05f;

    [Header("Timing (seconds)")]
    public float hitboxOnTime = 1.0f;
    public float hitboxOffTime = 0.32f;
    public float totalTime = 0.55f;             // attack lock süresi

    [Header("Gameplay")]
    public int damage = 25;
    public float moveLockTime = 0.45f;          // souls-like commitment
    public float staminaCost = 20.0f;
}
