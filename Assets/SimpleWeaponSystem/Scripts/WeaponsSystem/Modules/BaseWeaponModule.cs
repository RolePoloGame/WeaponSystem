using System;
using UnityEngine;
using WeaponSystem.Core;

namespace WeaponSystem.Modules
{
    public abstract class BaseWeaponModule : RuntimeScriptableObject
    {
        public abstract bool CanPerform(Types.BaseWeaponType weapon);
        public virtual void OnStartPerform(Types.BaseWeaponType weapon) { }
        public virtual void OnEndPerform(Types.BaseWeaponType weapon) { }

        public override event Action OnChanged;
        protected void OnModuleChanged()
        {
            OnChanged?.Invoke();
        }

        public virtual void Tick(float timeDelta)
        {

        }

        public virtual void OnShow()
        {

        }

        public virtual void OnHide()
        {

        }
    }
}
