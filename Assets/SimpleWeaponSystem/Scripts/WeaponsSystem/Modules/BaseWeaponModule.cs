using System;
using WeaponSystem.Core;

namespace WeaponSystem.Modules
{
    public abstract class BaseWeaponModule : RuntimeScriptableObject
    {
        /// <summary>
        /// Runs internall allowance for Weapon to perform an action.
        /// </summary>
        /// <param name="weapon">Weapon that is being verified</param>
        /// <returns>true if module allows for Action perform</returns>
        public abstract bool CanPerform(Types.BaseWeaponType weapon);
        /// <summary>
        /// Is called right before Action is performed
        /// </summary>
        /// <param name="weapon">Weapon that performs an Action</param>
        public virtual void OnStartPerform(Types.BaseWeaponType weapon) { }
        /// <summary>
        /// Is called right after Action is performed
        /// </summary>
        /// <param name="weapon">Weapon that performs an Action</param>
        public virtual void OnEndPerform(Types.BaseWeaponType weapon) { }

        /// <summary>
        /// Is invoked when module is modified
        /// </summary>
        public override event Action OnChanged;


        /// <summary>
        /// Invokes <see cref="OnChanged"/>
        /// </summary>
        protected void OnModuleChanged()
        {
            OnChanged?.Invoke();
        }

        /// <summary>
        /// Is called every frame when is an actively selected weapon
        /// </summary>
        public virtual void Tick(float timeDelta)
        {

        }
        /// <summary>
        /// Is called when weapon owning this module is selected
        /// </summary>
        public virtual void OnShow()
        {

        }
        /// <summary>
        /// Is called when weapon owning this module is deselected
        /// </summary>
        public virtual void OnHide()
        {

        }
    }
}
