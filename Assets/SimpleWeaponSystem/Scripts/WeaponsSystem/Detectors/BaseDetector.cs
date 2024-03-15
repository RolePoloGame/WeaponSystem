using System;
using UnityEngine;
using WeaponSystem.Interfaces;

namespace WeaponSystem.Detectors
{
    public abstract class BaseDetector : ScriptableObject
    {
        public delegate void DetectedAction(IDetectable[] targets, Vector3 hitPoint);

        public virtual void TryDetect(DetectedAction action)
        {
            Debug.LogWarning($"Not implemented {GetType().Name}. Returning empty Array");
            action.Invoke(Array.Empty<IDetectable>(), Vector3.zero);
        }
    }
}
