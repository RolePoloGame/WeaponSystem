using UnityEngine;
using WeaponSystem.Detectors;
using WeaponSystem.Interfaces;
using WeaponSystem.Types;

namespace WeaponSystem.Actions
{
    public class SliceAction : BaseAction
    {
        protected override void OnPerformOnTarget(IDetectable target, BaseWeaponType weapon, Vector3 hitPoint)
        {
            Debug.Log("Slice!");
        }
    }
}
