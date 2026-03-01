using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI HealthText;

    void Start()
    {
        var stats = FindObjectOfType<CharacterStats>();
        if (stats)
        {
            stats.OnStaminaChanged += OnStaminaChanged;
            OnStaminaChanged(stats.Stamina, stats.maxStamina);

            stats.OnHealthChanged += OnHealthChanged;
            OnHealthChanged(stats.HP, stats.maxHP);
        }

    }

    void OnDestroy()
    {
        var stats = FindObjectOfType<CharacterStats>();
        if (stats)
        {
            stats.OnStaminaChanged -= OnStaminaChanged;
            stats.OnHealthChanged -= OnHealthChanged;
        }
    }

    void OnStaminaChanged(float current, float max)
    {
        staminaText.text = $"Stamina: {Mathf.CeilToInt(current)} / {max}";
    }

    void OnHealthChanged(float current, float max)
    {
        HealthText.text = $"Health: {Mathf.CeilToInt(current)} / {max}";
    }
}
