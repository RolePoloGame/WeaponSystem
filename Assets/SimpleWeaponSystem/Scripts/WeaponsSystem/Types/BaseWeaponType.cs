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
        /// <summary>
        /// Is called when weapon is deselected. Calls <see cref="BaseAction.OnHide"/>
        /// </summary>
        public void OnHide() => Action.OnHide();
        /// <summary>
        /// Is called when weapon is deselected. Calls <see cref="BaseAction.OnShow"/>
        /// </summary>
        public void OnShow() => Action.OnShow();

        public override void OnInitialize()
        {
            this.Action = Initialize(Action);
            Action.OnChanged += () => OnChanged?.Invoke();
        }

        /// <summary>
        /// Is called every frame. Calls <see cref="BaseWeaponModule.Tick(float)"/>
        /// </summary>
        /// <param name="timeDelta">Use <see cref="Time.deltaTime"/> or your custom deltaTime</param>
        public void TickModules(float timeDelta)
        {
            foreach (var module in Action.Modules)
                module.Tick(timeDelta);
        }

        /// <summary>
        /// Tries to find first instance of type derived from <see cref="BaseWeaponModule"/> 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ammunitionModule">If succesfull, contains found instance, otherwise defaults to a default value of <see cref="BaseWeaponModule"/></param>
        /// <returns>True if instance was found</returns>
        public bool TryGetModule<T>(out T ammunitionModule) where T : BaseWeaponModule
        {
            ammunitionModule = null;
            if (Action == null) return false;
            if (Action.Modules == null) return false;
            ammunitionModule = Action.Modules.FirstOrDefault(x => x.GetType() == typeof(T)) as T;
            return ammunitionModule != null;
        }

        /// <summary>
        /// Calls <see cref="BaseAction.TryPerformAction(BaseWeaponType)"/> on a given Action.
        /// </summary>
        /// <param name="inputAction">Ignored parameter until implemented</param>
        /// <returns>True if action was performed</returns>
        public bool TryPerform(EInputAction inputAction)
        {
            if (Action == null)
            {
                Debug.LogError("Action was not assigned");
                return false;
            }
            //TODO: Expand for more than one Action per WeaponType
            return PerformActionInternal(Action);
        }

        private bool PerformActionInternal(BaseAction action) => action.TryPerformAction(this);
    }

}