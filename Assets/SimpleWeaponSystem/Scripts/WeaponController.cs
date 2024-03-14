using System;
using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.GameManagement;
using WeaponSystem.Types;

namespace WeaponSystem.Controllers
{
    public class WeaponController : MonoBehaviour
    {
        public List<BaseWeaponType> Weapons { get; private set; }
        private BaseWeaponType CurrentWeapon;
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
            InputManager.Instance.OnPrimaryPerformed += PerformPrimary;
            InputManager.Instance.OnPrimaryPerformed += PerformSecondary;
        }

        private void OnDisable()
        {
            InputManager.Instance.OnPrimaryPerformed -= PerformPrimary;
            InputManager.Instance.OnPrimaryPerformed -= PerformSecondary;
        }

        private void PerformPrimary()
        {
            CurrentWeapon.TryPerform(Actions.EInputAction.Primary);
        }

        private void PerformSecondary()
        {
            UpdateWeapon();
        }

        private void UpdateWeapon()
        {
            if (currentIndex >= Weapons.Count)
                currentIndex = 0;
            CurrentWeapon = Weapons[currentIndex];
        }


        private void LoadWeapons()
        {
            Database.Instance.OnDatabaseLoaded -= LoadWeapons;
            Weapons = Database.Instance.WeaponTypes;
            if (Weapons.Count == 0) return;
            currentIndex = 0;
            UpdateWeapon();
        }
    }
}
