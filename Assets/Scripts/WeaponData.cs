using UnityEngine;

[CreateAssetMenu(menuName = "Combat/Weapon Data")]
public class WeaponData : ScriptableObject
{
    public AttackData lightAttack;
    // sonra eklersin:
    // public AttackData heavyAttack;
    // public AttackData[] comboLight;
}
