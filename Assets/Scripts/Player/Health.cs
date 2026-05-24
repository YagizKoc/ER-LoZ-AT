using UnityEngine;

public class Health : MonoBehaviour
{
    public int hp = 100;

    public void TakeDamage(int dmg)
    {
        // Eski çağrılar için: saldırgan bilinmiyorsa reaction yapmadan düşür
        ApplyDamage(dmg);

        // İstersen burada reaction yapma (attacker bilinmiyor)
        // var react = GetComponent<EnemyHitReaction>();
        // if (react) react.ReactToHit(transform.position); // önerilmez
    }

    public void TakeDamage(int dmg, Vector3 attackerPosition)
    {
        ApplyDamage(dmg);
    }

    void ApplyDamage(int dmg)
    {
        hp -= dmg;
        Debug.Log(gameObject.name + " HP: " + hp);

        if (hp <= 0)
            Destroy(gameObject);
    }
}
