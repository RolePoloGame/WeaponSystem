using UnityEngine;
using WeaponSystem.Types;

namespace WeaponSystem.Modules
{
    [CreateAssetMenu(fileName = "new CooldownModule", menuName = "WeaponSystem/Module/Cooldown")]

    public class CooldownModule : BaseWeaponModule
    {
        public override bool CanPerform(BaseWeaponType weapon)
        {
            return true;
        }
    }
}
