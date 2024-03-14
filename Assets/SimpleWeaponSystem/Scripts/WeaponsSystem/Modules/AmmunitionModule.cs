using UnityEngine;
using WeaponSystem.Types;

namespace WeaponSystem.Modules
{
    [CreateAssetMenu(fileName = "new AmmunitionModule", menuName = "WeaponSystem/Module/Ammunition")]
    public class AmmunitionModule : BaseWeaponModule
    {
        public override bool CanPerform(BaseWeaponType weapon)
        {
            return true;
        }
    }
}
