using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AddressableAssets;
using UnityEngine.InputSystem;
using WeaponSystem.Core;
using WeaponSystem.Types;
namespace WeaponSystem.GameManagement
{
    public class Database : SingletonController<Database>
    {
        public List<BaseWeaponType> WeaponTypes { get; set; }

        public event Action OnDatabaseLoaded;
        public bool IsLoaded = false;
        private void Start()
        {
            StartCoroutine(LoadDatabase());
        }

        private IEnumerator LoadDatabase()
        {
            var weaponsHandle = Addressables.LoadAssetsAsync<BaseWeaponType>(nameof(WeaponTypes), WeaponTypes.Add);
            yield return weaponsHandle;

            OnDatabaseLoaded?.Invoke();
            IsLoaded = true;
        }
    }
}