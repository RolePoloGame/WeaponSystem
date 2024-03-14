using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Ammunition;
using WeaponSystem.Types;

namespace WeaponSystem.Modules
{
    [CreateAssetMenu(fileName = "new AmmunitionModule", menuName = "WeaponSystem/Module/Ammunition")]
    public class AmmunitionModule : BaseWeaponModule
    {
        [SerializeField]
        private List<AmmunitionType> allowedAmmoType = new();

        [SerializeField, Tooltip("If set to 0, no limit is present")]
        private int maxAmmunition = 0;

        [SerializeField, Tooltip("If set to 0, no limit is present")] private int magazineSize = 0;


        [SerializeField] private int ammoPerShot = 0;
        [SerializeField] private bool autoReload = true;
        [SerializeField] private bool wastesAmmoOnReload = false;

        [SerializeField]
        private float reloadTime = 0.2f;

        //Used only at runtime
        private int currentAmmunition = 0;
        private int currentMagazine = 0;

        public override void OnInitialize()
        {
            currentAmmunition = 0;
            currentMagazine = 0;
        }

        public override bool CanPerform(BaseWeaponType weapon)
        {
            return ammoPerShot >= currentMagazine;
        }

        public override void OnStartPerform(BaseWeaponType weapon)
        {
            currentMagazine -= ammoPerShot;
        }

        public override void OnEndPerform(BaseWeaponType weapon)
        {
            if (autoReload)
                Reload();
        }

        public void Reload()
        {
            if (!CanReload()) return;
            if (wastesAmmoOnReload)
                currentMagazine = 0;
            int emptySpaceInMagazine = (magazineSize - currentMagazine);
            var reloadAmount = currentAmmunition < emptySpaceInMagazine ? currentAmmunition : currentAmmunition - emptySpaceInMagazine;

            currentMagazine += reloadAmount;
            currentAmmunition -= reloadAmount;
        }

        private bool CanReload()
        {
            var hasMagazines = magazineSize > 0;
            var magazineNeedsReload = currentMagazine < magazineSize;
            var hasAmmunition = currentAmmunition > 0;

            return hasMagazines && hasAmmunition && magazineNeedsReload;
        }

        public bool CanPickAmmo(AmmunitionType type)
        {
            if (maxAmmunition == 0) return true;
            if (allowedAmmoType.Count == 0) return false;
            if (!allowedAmmoType.Contains(type)) return false;
            return currentAmmunition < maxAmmunition;
        }

        public void AddAmmo(int ammo)
        {
            if (maxAmmunition == 0)
            {
                currentAmmunition += ammo;
                return;
            }

            if (ammo + currentAmmunition > maxAmmunition)
            {
                currentAmmunition = maxAmmunition;
                return;
            }

            currentAmmunition += ammo;
        }
    }
}
