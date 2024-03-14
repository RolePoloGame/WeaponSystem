using UnityEngine;

namespace WeaponSystem.Ammunition
{
    [CreateAssetMenu(fileName = "new AmmunitionType", menuName = "WeaponSystem/Ammunition Type")]
    public class AmmunitionType : ScriptableObject
    {
        [field: SerializeField]
        public string Name { get; private set; }
        [field: SerializeField]
        public Texture2D Icon { get; private set; }
    }
}
