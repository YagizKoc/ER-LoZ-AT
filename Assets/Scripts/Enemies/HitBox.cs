using UnityEngine;

public class HitBox : MonoBehaviour
{
    public int damage = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player hit!");

            // Player objesinden CharacterStats veya IDamageable al
            CharacterStats characterStats = other.GetComponentInParent<CharacterStats>();

            if (characterStats != null)
            {
                characterStats.TakeDamage(damage);
            }
            else
            {
                Debug.LogWarning("Player'da CharacterStats bulunamadı!");
            }
        }
    }
}