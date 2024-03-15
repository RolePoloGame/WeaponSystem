using UnityEngine;

namespace WeaponSystem.Core
{
    public abstract class SingletonController : MonoBehaviour
    {

    }

    public class SingletonController<T> : SingletonController where T : SingletonController
    {

        public static T Instance { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this as T;
                return;
            }
            Debug.LogError($"Instance of {nameof(SingletonController<T>)} already exists on {Instance.gameObject.name}. Disabling script on {gameObject.name}...");
            enabled = false;
        }
    }
}