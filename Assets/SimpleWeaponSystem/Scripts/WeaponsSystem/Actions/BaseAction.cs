using RolePoloGame;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Detectors;
using WeaponSystem.Interfaces;
using WeaponSystem.Modules;
using WeaponSystem.Types;

namespace WeaponSystem.Actions
{
    public abstract class BaseAction : RuntimeScriptableObject
    {
        [field: SerializeField]
        protected BaseDetector Detection { get; private set; }

        [field: SerializeField]
        public List<BaseWeaponModule> Modules { get; protected set; }

        public int ModulesStatus => modulesStatus;
        private int modulesStatus;

        public override void OnInitialize()
        {
            foreach (var module in Modules)
                Initialize(module);
        }

        public virtual float CalculateDamage(BaseWeaponType weapon)
        {
            return weapon.BaseDamage;
        }

        /// <summary>
        /// Tries to perform action if possible.
        /// </summary>
        /// <returns>True if action was performed</returns>
        public bool TryPerformAction(BaseWeaponType weapon)
        {
            if (!CanPerform(weapon)) return false;
            OnPerformAction(weapon);
            return true;
        }

        protected void OnPerformAction(BaseWeaponType weapon)
        {
            Detection.TryDetect((detected, hitPoint) =>
            {
                Debug.Log($"Performing {GetType().Name} action on {detected.Length} targets. Hit at [{hitPoint}]");
                foreach (var target in detected)
                    OnPerformOnTarget(target, weapon, hitPoint);
            });
        }
        protected abstract void OnPerformOnTarget(IDetectable target, BaseWeaponType weapon, Vector3 hitPoint);

        public List<BaseWeaponModule> GetFailedModules()
        {
            List<BaseWeaponModule> failedModules = new();
            for (int i = 0; i < Modules.Count; i++)
            {
                var didModuleFail = (modulesStatus & (1 << i - 1)) != 0;
                if (!didModuleFail)
                    continue;
                failedModules.Add(Modules[i]);
            }
            return failedModules;
        }


        /// <summary>
        /// Method goes through all <see cref="BaseWeaponModule"/> in <see cref="Modules"/> 
        /// calling <see cref="BaseWeaponModule.CanPerform"/> on each. Stores result of the check in <see cref="modulesStatus"/>
        /// </summary>
        /// <returns>true if all modules return true</returns>
        private bool CanPerform(Types.BaseWeaponType weapon)
        {
            if (Modules.Count == 0) return true;
            modulesStatus = 0;
            for (var i = 0; i < Modules.Count; i++)
            {
                var module = Modules[i];
                var canPerform = module.CanPerform(weapon);
                if (canPerform) continue;
                modulesStatus |= 1 << i;
            }
            return modulesStatus == 0;
        }
    }
}
