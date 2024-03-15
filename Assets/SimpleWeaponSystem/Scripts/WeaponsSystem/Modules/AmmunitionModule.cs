using System.Collections.Generic;
using UnityEngine;
using WeaponSystem.Ammunition;
using WeaponSystem.Types;

namespace WeaponSystem.Modules
{
    [CreateAssetMenu(fileName = "new AmmunitionModule", menuName = "WeaponSystem/Module/Ammunition")]
    public class AmmunitionModule : BaseWeaponModule
    {
        [field: SerializeField]
        public List<AmmunitionType> AllowedAmmoType { get; private set; } = new();

        [field: SerializeField, Tooltip("If set to 0, no limit is present")]
        public int MaxAmmunition { get; private set; } = 0;

        [field: SerializeField, Tooltip("If set to 0, no limit is present")]
        public int MagazineSize { get; private set; } = 0;
        [field: SerializeField]
        public int AmmoPerShot { get; private set; } = 0;
        [field: SerializeField]
        public bool AutoReloadOnEmpty { get; private set; } = true;
        [field: SerializeField]
        public bool WastesAmmoOnReload { get; private set; } = false;

        [field: SerializeField]
        public float ReloadTime { get; private set; } = 0.2f;

        //Used only at runtime
        public int CurrentAmmunition { get; private set; } = 0;
        public int CurrentMagazine { get; private set; } = 0;
        public bool IsReloading => isReloadActive;
        public bool IsTimerFinished => reloadTimer < 0.0f;

        private float reloadTimer = 0.0f;
        private bool isReloadActive = false;

        public override void Tick(float timeDelta)
        {
            if (!isReloadActive) return;
            reloadTimer -= timeDelta;
            if (!IsTimerFinished) return;
            ApplyReload();
        }

        public override void OnInitialize()
        {
            CurrentAmmunition = 0;
            CurrentMagazine = 0;
        }

        public override void OnHide()
        {
            CancelReload();

        }

        private void CancelReload()
        {
            if (!IsReloading) return;
            isReloadActive = false;
            reloadTimer = 0.0f;
        }

        public override bool CanPerform(BaseWeaponType weapon)
        {
            if (IsReloading) return false;
            return (MagazineSize > 0 && CurrentMagazine >= AmmoPerShot) || CurrentAmmunition >= AmmoPerShot;
        }

        public override void OnStartPerform(BaseWeaponType weapon)
        {
            CancelReload();
            if (MagazineSize > 0)
                CurrentMagazine -= AmmoPerShot;
            else
                CurrentAmmunition -= AmmoPerShot;
        }

        public override void OnEndPerform(BaseWeaponType weapon)
        {
            OnModuleChanged();
            if (AutoReloadOnEmpty && MagazineSize > 0 && CurrentMagazine == 0)
                Reload();
        }

        public void Reload()
        {
            if (!CanReload()) return;
            isReloadActive = true;
            reloadTimer = ReloadTime;
        }

        private void ApplyReload()
        {
            if (WastesAmmoOnReload)
                CurrentMagazine = 0;
            int emptySpaceInMagazine = (MagazineSize - CurrentMagazine);

            var reloadAmount = CurrentAmmunition < emptySpaceInMagazine ? CurrentAmmunition : emptySpaceInMagazine;

            CurrentMagazine += reloadAmount;
            CurrentAmmunition -= reloadAmount;


            if (CurrentAmmunition < 0) CurrentAmmunition = 0;
            if (CurrentMagazine < 0) CurrentMagazine = 0;
            if (MaxAmmunition < 0) CurrentAmmunition = 0;

            isReloadActive = false;
            reloadTimer = 0.0f;
            OnModuleChanged();
            Debug.Log("Reload performed");
        }

        private bool CanReload()
        {
            var hasMagazines = MagazineSize > 0;
            var magazineNeedsReload = CurrentMagazine < MagazineSize;
            var hasAmmunition = CurrentAmmunition > 0;

            return !IsReloading && hasMagazines && hasAmmunition && magazineNeedsReload;
        }

        private bool CanAddAmmo(AmmunitionType type)
        {
            if (MaxAmmunition == 0) return true;
            if (AllowedAmmoType.Count == 0) return false;
            if (!AllowedAmmoType.Contains(type)) return false;
            return CurrentAmmunition < MaxAmmunition;
        }

        public void AddAmmo(AmmunitionType type, int ammo)
        {
            if (!CanAddAmmo(type)) return;
            if (MaxAmmunition == 0)
            {
                CurrentAmmunition += ammo;
                OnModuleChanged();
                return;
            }

            if (ammo + CurrentAmmunition > MaxAmmunition)
            {
                CurrentAmmunition = MaxAmmunition;
                OnModuleChanged();
                return;
            }

            CurrentAmmunition += ammo;
            OnModuleChanged();
        }

        public float GetReloadRatio()
        {
            if (!IsReloading) return 0.0f;
            return reloadTimer / ReloadTime;
        }
    }
}
