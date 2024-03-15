using System;
using UnityEngine;

namespace WeaponSystem.Core
{
    /// <summary>
    /// This class eases the usage of ScriptableObjects as independend instances during runtime. 
    /// Use <see cref="Initialize{T}(T)"> method to create an instance of a given ScriptableObject
    /// </summary>
    public abstract class RuntimeScriptableObject : ScriptableObject
    {
        public abstract event Action OnChanged;
        public bool IsInitialized => _isInitialized;
        private bool _isInitialized = false;
        public virtual void OnInitialize() { }
        /// <summary>
        /// Creates a new instance of a given derived class of a <see cref="RuntimeScriptableObject"/> and calls OnInitialize on it.
        /// </summary>
        /// <typeparam name="T">Must derive from <see cref="RuntimeScriptableObject"/></typeparam>
        /// <param name="source"></param>
        /// <returns>Created Instance</returns>
        public static T Initialize<T>(T source) where T : RuntimeScriptableObject
        {
            T instance = Instantiate(source);
            instance.OnInitialize();
            instance._isInitialized = true;
            return instance;
        }
    }
}
