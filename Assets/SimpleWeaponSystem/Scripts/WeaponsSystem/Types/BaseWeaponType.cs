using RolePoloGame;
using System;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Actions;
using WeaponSystem.Modules;

namespace WeaponSystem.Types
{
    public abstract class BaseWeaponType : RuntimeScriptableObject
    {
        public override void OnInitialize()
        {
            foreach (var module in Modules)
                Initialize(module);

            foreach (var module in Actions)
                Initialize(module.Action);
        }

        [field: SerializeField]
        public string Name { get; protected set; } = string.Empty;
        [field: SerializeField]
        public string Description { get; protected set; } = string.Empty;

        [field: SerializeField]
        public int BaseDamage { get; protected set; } = 0;

        [field: SerializeField]
        public List<BaseWeaponModule> Modules { get; protected set; }

        [field: SerializeField]
        protected List<ActionInputPair> Actions { get; set; }

        private void PerformAction(BaseAction action)
        {
            action.TryPerformAction(this);
        }

        [Serializable]
        protected class ActionInputPair
        {
            [field: SerializeField]
            public object Input;

            [field: SerializeField]
            public BaseAction Action;
        }
    }

}