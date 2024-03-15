using UnityEngine;

namespace WeaponSystem.Interfaces
{
    public interface IDetectable
    {
        public GameObject gameObject { get; }
        public Transform transform { get; }
    }
}
