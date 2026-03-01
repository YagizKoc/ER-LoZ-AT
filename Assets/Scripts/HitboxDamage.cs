using UnityEngine;
using System.Collections.Generic;

public class HitboxDamage : MonoBehaviour
{
    public int damage = 10;
    public Transform attacker;

    readonly HashSet<Health> hitThisAttack = new HashSet<Health>();

    public void ResetHits()
    {
        hitThisAttack.Clear();
    }

    void OnTriggerEnter(Collider other) => TryDamage(other);
    void OnTriggerStay(Collider other) => TryDamage(other);

    void TryDamage(Collider other)
    {
        var health = other.GetComponentInParent<Health>();
        if (!health) return;

        if (hitThisAttack.Contains(health)) return;
        hitThisAttack.Add(health);

        Vector3 attackerPos = attacker ? attacker.position : transform.position;
        health.TakeDamage(damage, attackerPos);
    }

    public void Arm(int dmg, Transform atk)
    {
        damage = dmg;
        attacker = atk;
        ResetHits();
        gameObject.SetActive(true);
    }

    public void Disarm()
    {
        gameObject.SetActive(false);
    }

}
