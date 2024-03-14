using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WeaponSystem.Actions;
using WeaponSystem.Core;
using WeaponSystem.Modules;

namespace WeaponSystem.Types
{
    public abstract class BaseWeaponType : RuntimeScriptableObject
    {
        [field: SerializeField]
        public string Name { get; protected set; } = "new Weapon";
        [field: SerializeField]
        public Texture2D Icon { get; protected set; }

        [field: SerializeField]
        public string Description { get; protected set; } = string.Empty;

        [field: SerializeField]
        public int BaseDamage { get; protected set; } = 0;

        [field: SerializeField]
        public List<BaseWeaponModule> Modules { get; protected set; }

        [field: SerializeField]
        protected List<ActionInputPair> Actions { get; set; }

        public override void OnInitialize()
        {
            foreach (var module in Modules)
                Initialize(module);

            foreach (var module in Actions)
                Initialize(module.Action);
        }

        public void TryPerform(EInputAction inputAction)
        {
            var action = Actions.FirstOrDefault(x => x.Input == inputAction);
            if (action == null) return;
            PerformAction(action.Action);
        }

        private void PerformAction(BaseAction action)
        {
            action.TryPerformAction(this);
        }

        [Serializable]
        protected class ActionInputPair
        {
            [field: SerializeField]
            public EInputAction Input;

            [field: SerializeField]
            public BaseAction Action;
        }
    }

}