using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Core;
using WeaponSystem.GameManagement;
using WeaponSystem.Modules;
using WeaponSystem.Types;

namespace WeaponSystem.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        private List<BaseWeaponType> Weapons { get; set; } = new();
        public BaseWeaponType CurrentWeapon => currentWeapon;

        private BaseWeaponType currentWeapon;

        private int currentIndex = -1;

        private void Start()
        {
            if (Database.Instance.IsLoaded)
                LoadWeapons();
            else
                Database.Instance.OnDatabaseLoaded += LoadWeapons;
        }

        private void OnEnable()
        {
            if (InputManager.Instance == null) return;
            InputManager.Instance.OnPrimaryPerformed += PerformPrimary;
            InputManager.Instance.OnSecondaryPerformed += PerformSecondary;
            InputManager.Instance.OnReloadPerformed += PerformReload;
            InputManager.Instance.OnUsePerformed += PerformUse;
        }

        private void OnDisable()
        {
            if (InputManager.Instance == null) return;
            InputManager.Instance.OnPrimaryPerformed -= PerformPrimary;
            InputManager.Instance.OnSecondaryPerformed -= PerformSecondary;
            InputManager.Instance.OnReloadPerformed -= PerformReload;
            InputManager.Instance.OnUsePerformed -= PerformUse;
        }

        private void Update()
        {
            if (currentWeapon == null)
                return;
            currentWeapon.TickModules(Time.deltaTime);
        }

        private void PerformPrimary()
        {
            currentWeapon.TryPerform(Actions.EInputAction.Primary);
        }

        private void PerformSecondary()
        {
            currentIndex++;
            UpdateWeapon();
        }

        private void PerformUse()
        {
            DebugAddAmmo();
        }

        private void PerformReload()
        {
            if (currentWeapon.TryGetModule<AmmunitionModule>(out var module))
                module.Reload();
        }

        private void DebugAddAmmo()
        {
            if (!currentWeapon.TryGetModule<AmmunitionModule>(out var module)) return;
            var index = Random.Range(0, module.AllowedAmmoType.Count);
            var type = module.AllowedAmmoType[index];
            int amount = 30;
            Debug.Log($"Adding {amount} {type.Name} ammo");
            module.AddAmmo(type, amount);
        }

        private void UpdateWeapon()
        {
            if (currentIndex >= Weapons.Count)
                currentIndex = 0;
            Debug.Log($"Switching to next weapon");
            if (currentWeapon != null) currentWeapon.OnHide();
            currentWeapon = Weapons[currentIndex];
            if (currentWeapon != null) currentWeapon.OnShow();
        }


        private void LoadWeapons()
        {
            Database.Instance.OnDatabaseLoaded -= LoadWeapons;
            Weapons = Database.Instance.WeaponTypes;
            for (var i = 0; i < Weapons.Count; i++)
                Weapons[i] = RuntimeScriptableObject.Initialize(Weapons[i]);
            if (Weapons.Count == 0) return;
            currentIndex = 0;
            UpdateWeapon();
        }
    }
}
