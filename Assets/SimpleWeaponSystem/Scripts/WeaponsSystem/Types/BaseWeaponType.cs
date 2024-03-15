using System;
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
        protected BaseAction Action { get; set; }

        public override event Action OnChanged;

        public void OnHide() => Action.OnHide();
        public void OnShow() => Action.OnShow();

        public override void OnInitialize()
        {
            this.Action = Initialize(Action);
            Action.OnChanged += () => OnChanged?.Invoke();
        }


        public void TickModules(float timeDelta)
        {
            foreach (var module in Action.Modules)
                module.Tick(timeDelta);
        }

        public bool TryGetModule<T>(out T ammunitionModule) where T : BaseWeaponModule
        {
            ammunitionModule = null;
            if (Action == null) return false;
            if (Action.Modules == null) return false;
            ammunitionModule = Action.Modules.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
            return ammunitionModule != null;
        }

        public void TryPerform(EInputAction inputAction)
        {
            PerformActionInternal(Action);
        }

        private void PerformActionInternal(BaseAction action) => action.TryPerformAction(this);

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