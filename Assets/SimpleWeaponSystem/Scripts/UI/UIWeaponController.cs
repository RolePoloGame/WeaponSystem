using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using WeaponSystem.Controllers;
using WeaponSystem.Modules;
using WeaponSystem.Types;

namespace WeaponSystem.UI
{
    public class UIWeaponController : MonoBehaviour
    {
        [SerializeField]
        private WeaponController weaponController;
        private BaseWeaponType currentWeapon;

        [Header("Info")]
        [SerializeField]
        private TextMeshProUGUI weaponNameTMP;
        [SerializeField]
        private RawImage weaponIconRI;
        [SerializeField]
        private TextMeshProUGUI currentAmmoTMP;
        [SerializeField]
        private TextMeshProUGUI maxAmmoTMP;
        [SerializeField]
        private TextMeshProUGUI currentMagazineTMP;
        [SerializeField]
        private TextMeshProUGUI maxMagazineTMP;
        private bool needsUpdate = true;

        [SerializeField]
        private Image reloadImage;
        private AmmunitionModule ammunitionModule;

        private bool HasWeapon => currentWeapon != null;

        private void Start()
        {
            if (weaponController != null)
                return;

            Debug.LogError($"{nameof(weaponController)} not assigned.");
            enabled = false;
            return;
        }


        private void Update()
        {
            TryGetWeapon();
            if (!HasWeapon)
                return;
            if (needsUpdate)
                UpdateWeaponGUI();
            UpdateReload();
        }

        private void UpdateReload()
        {
            if (ammunitionModule == null)
                return;
            reloadImage.fillAmount = ammunitionModule.GetReloadRatio();
        }

        private void TryGetWeapon()
        {
            if (weaponController == null) return;
            if (currentWeapon == null)
            {
                GetWeapon();
                return;
            }
            if (currentWeapon == weaponController.CurrentWeapon) return;
            GetWeapon();
        }

        private void GetWeapon()
        {
            OnReleaseWeapon();
            currentWeapon = weaponController.CurrentWeapon;
            OnNewWeapon();
            needsUpdate = true;
        }

        private void OnNewWeapon()
        {
            if (currentWeapon != null)
            {
                currentWeapon.OnChanged += HandleOnChanged;
                currentWeapon.TryGetModule(out ammunitionModule);
            }

            reloadImage.fillAmount = 0;
        }

        private void OnReleaseWeapon()
        {
            if (currentWeapon != null)
                currentWeapon.OnChanged -= HandleOnChanged;
        }

        private void HandleOnChanged()
        {
            needsUpdate = true;
        }

        private void UpdateWeaponGUI()
        {
            needsUpdate = false;
            weaponNameTMP.SetText(currentWeapon.Name);
            var hasAmmoModule = ammunitionModule != null;

            currentAmmoTMP.gameObject.SetActive(hasAmmoModule);
            currentMagazineTMP.gameObject.SetActive(hasAmmoModule);
            maxAmmoTMP.gameObject.SetActive(hasAmmoModule);
            maxMagazineTMP.gameObject.SetActive(hasAmmoModule);

            weaponIconRI.texture = currentWeapon.Icon;
            if (!hasAmmoModule)
                return;

            int magazineSize = ammunitionModule.MagazineSize;

            var usesMagazine = magazineSize > 0;
            int maxAmmunition = ammunitionModule.MaxAmmunition;
            int currentAmmunition = ammunitionModule.CurrentAmmunition;
            int currentMagazine = ammunitionModule.CurrentMagazine;

            var hasMaxAmmo = maxAmmunition > 0;

            string currentAmmoStr = currentAmmunition.ToString();
            string maxAmmoStr = maxAmmunition.ToString();
            string currentMagazineStr = currentMagazine.ToString();
            string maxMagazineStr = magazineSize.ToString();

            currentAmmoTMP.SetText(usesMagazine ? currentAmmoStr : string.Empty);
            maxAmmoTMP.SetText(hasMaxAmmo ? maxAmmoStr : string.Empty);

            currentMagazineTMP.SetText(usesMagazine ? currentMagazineStr : currentAmmoStr);
            maxMagazineTMP.SetText(usesMagazine ? maxMagazineStr : string.Empty);
        }
    }
}