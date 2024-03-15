using UnityEngine;
using WeaponSystem.Interfaces;
using WeaponSystem.Types;

namespace WeaponSystem.Actions
{
    [CreateAssetMenu(fileName = "new ShootAction", menuName = "WeaponSystem/Action/Shoot")]
    public class ShootAction : BaseAction
    {
        [field: SerializeField]
        protected float DamageMultiplier = 1.0f;

        protected override void OnPerformOnTarget(IDetectable target, BaseWeaponType weapon, Vector3 hitPoint)
        {
            Debug.Log("Bang");
        }

        public override float CalculateDamage(BaseWeaponType weapon)
        {
            return weapon.BaseDamage * DamageMultiplier;
        }
    }
}
