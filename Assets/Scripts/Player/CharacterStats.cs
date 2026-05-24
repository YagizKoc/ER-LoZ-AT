using UnityEngine;
using System;
using TMPro;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] TMP_Text healthText;

    [Header("Stamina")]
    public float maxStamina = 100f;
    public float staminaRegenPerSec = 20f;

    [Header("Health")]
    public int maxHP = 100;
    public int HP { get; private set; }

    public bool IsDead => HP <= 0;


    public float Stamina { get; private set; }

    public event Action<float, float> OnStaminaChanged; // current, max
    public event Action<float, float> OnHealthChanged; // current, max

    void Awake()
    {
        HP = maxHP;
        Stamina = maxStamina;
        OnStaminaChanged?.Invoke(Stamina, maxStamina);
        OnHealthChanged?.Invoke(HP, maxHP);
    }

    public void Tick(float dt)
    {
        if (Stamina >= maxStamina) return;

        Stamina = Mathf.Min(maxStamina, Stamina + staminaRegenPerSec * dt);
        OnStaminaChanged?.Invoke(Stamina, maxStamina);
    }

    public bool TrySpendStamina(float amount)
    {
        if (amount <= 0f) return true;
        if (Stamina < amount) return false;

        Stamina -= amount;
        OnStaminaChanged?.Invoke(Stamina, maxStamina);
        return true;
    }

    public void TakeDamage(int dmg)
    {
        // I-FRAME KONTROLÜ
        var invul = GetComponent<PlayerInvulnerability>();
        if (invul && invul.IsInvulnerable)
            return;

        HP -= dmg;
        Debug.Log("Player HP: " + HP);
        OnHealthChanged?.Invoke(HP, maxHP);

        if (HP <= 0)
        {
            Debug.Log("Player Dead");
            // sonra: respawn / death screen
        }
    }

}
