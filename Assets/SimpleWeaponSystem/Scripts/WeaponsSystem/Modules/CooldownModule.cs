using UnityEngine;
using WeaponSystem.Types;

namespace WeaponSystem.Modules
{
    [CreateAssetMenu(fileName = "new CooldownModule", menuName = "WeaponSystem/Module/Cooldown")]

    public class CooldownModule : BaseWeaponModule
    {
        [SerializeField]
        private bool isActiveOnShow = false;
        public bool IsCooledDown => isCooldownActive;

        [field: SerializeField]
        public float CooldownTime { get; private set; }

        private float reloadTimer = 0.0f;
        private bool isCooldownActive = false;
        public override bool CanPerform(BaseWeaponType weapon) => IsCooledDown;

        public override void Tick(float timeDelta)
        {
            if (!isCooldownActive) return;
            reloadTimer -= timeDelta;
            if (reloadTimer > 0.0f) return;
            isCooldownActive = false;
            reloadTimer = 0.0f;
        }

        public override void OnStartPerform(BaseWeaponType weapon)
        {
            if (isActiveOnShow)
                ActivateCooldown();
        }

        private void ActivateCooldown()
        {
            isCooldownActive = false;
            reloadTimer = CooldownTime;
        }

        public override void OnHide()
        {
            if (!isCooldownActive)
                return;
            ActivateCooldown();
        }
    }
}
