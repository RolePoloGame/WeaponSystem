using UnityEngine;
using WeaponSystem.Interfaces;
using WeaponSystem.Types;

namespace WeaponSystem.Actions
{
    [CreateAssetMenu(fileName = "new SliceAction", menuName = "WeaponSystem/Action/Slice")]
    public class SliceAction : BaseAction
    {
        protected override void OnPerformOnTarget(IDetectable target, BaseWeaponType weapon, Vector3 hitPoint)
        {
            Debug.Log("Slice!");
        }
    }
}
