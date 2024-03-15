using System;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Core;
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

        /// <summary>
        /// Is invoked when module is modified
        /// </summary>
        public override event Action OnChanged;
        public int ModulesStatus => modulesStatus;
        private int modulesStatus;

        public override void OnInitialize()
        {
            for (int i = 0; i < Modules.Count; i++)
            {
                Modules[i] = Initialize(Modules[i]);
                Modules[i].OnChanged += () => OnChanged?.Invoke();
            }
        }

        /// <summary>
        /// Calculates damage based on a given weapon.
        /// </summary>
        public virtual float CalculateDamage(BaseWeaponType weapon)
        {
            //TODO: Create Module Type that can modify damage
            return weapon.BaseDamage;
        }

        /// <summary>
        /// Tries to perform action if possible.
        /// </summary>
        /// <returns>True if action was performed</returns>
        public bool TryPerformAction(BaseWeaponType weapon)
        {
            if (!CanPerform(weapon)) return false;
            OnStartPerform(weapon);
            OnPerformAction(weapon);
            OnEndPerform(weapon);
            return true;
        }

        /// <summary>
        /// Is called right after Action is performed. 
        /// Calls <see cref="BaseWeaponModule.OnEndPerform(BaseWeaponType)"/> on each Module in <see cref="Modules"/>
        /// </summary>
        /// <param name="weapon">Weapon that performs an Action</param>
        private void OnEndPerform(BaseWeaponType weapon)
        {
            foreach (var module in Modules)
                module.OnEndPerform(weapon);
        }

        /// <summary>
        /// Is called right before Action is performed. 
        /// Calls <see cref="BaseWeaponModule.OnStartPerform(BaseWeaponType)"/> on each Module in <see cref="Modules"/>
        /// </summary>
        /// <param name="weapon">Weapon that performs an Action</param>
        private void OnStartPerform(BaseWeaponType weapon)
        {
            foreach (var module in Modules)
                module.OnStartPerform(weapon);
        }
        /// <summary>
        /// Is called when action is performed. Runs detection using <see cref="Detection"/> and calls <see cref="OnPerformOnTarget"/>
        /// Calls <see cref="BaseWeaponModule.OnStartPerform(BaseWeaponType)"/> on each Module in <see cref="Modules"/>
        /// </summary>
        /// <param name="weapon">Weapon that performs an Action</param>
        protected void OnPerformAction(BaseWeaponType weapon)
        {
            Detection.TryDetect((detected, hitPoint) =>
            {
                Debug.Log($"Performing {GetType().Name} action on {detected.Length} targets. Hit at [{hitPoint}]");
                foreach (var target in detected)
                    OnPerformOnTarget(target, weapon, hitPoint);
            });
        }
        /// <summary>
        /// Called when action is performed
        /// </summary>
        /// <param name="target"></param>
        /// <param name="weapon"></param>
        /// <param name="hitPoint"></param>
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
        private bool CanPerform(BaseWeaponType weapon)
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
        /// <summary>
        /// Is called when weapon is selected
        /// </summary>
        public void OnShow()
        {
            foreach (var module in Modules)
                module.OnShow();
        }
        /// <summary>
        /// Is called when weapon is deselected
        /// </summary>
        public void OnHide()
        {
            foreach (var module in Modules)
                module.OnHide();
        }
    }
}
