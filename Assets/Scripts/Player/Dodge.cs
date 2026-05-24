using UnityEngine;

public class Dodge : MonoBehaviour
{
    public float dodgeSpeed = 15f;
    public float dodgeTime = 2.5f;
    public float iFrameTime = 0.15f;
    PlayerInvulnerability invul;
    public bool IsDodging { get; private set; }

    Vector3 dodgeDir;
    float dodgeTimer;

    public float staminaCost = 25f;
    CharacterStats stats;

    void Awake()
    {
        stats = GetComponent<CharacterStats>();
        invul = GetComponent<PlayerInvulnerability>();
    }

    public void Tick(float dt)
    {
        if (!IsDodging) return;

        dodgeTimer -= dt;
        if (dodgeTimer <= 0f)
            IsDodging = false;
    }

    public bool TryStart(Vector3 dir)
    {
        if (stats && !stats.TrySpendStamina(staminaCost)) return false;

        if (IsDodging) return false;
        if (dir.sqrMagnitude < 0.01f) return false;

        dodgeDir = dir.normalized;
        dodgeTimer = dodgeTime;
        IsDodging = true;
        if (invul)
            invul.StartIFrame(iFrameTime);
        return true;
    }

    public Vector3 GetHorizontalVelocity(Vector3 normalDir, float normalSpeed)
    {
        if (IsDodging)
            return dodgeDir * dodgeSpeed;

        return normalDir * normalSpeed;
    }


}
