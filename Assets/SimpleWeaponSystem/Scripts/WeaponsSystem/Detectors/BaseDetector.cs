using System;
using UnityEngine;
using WeaponSystem.Interfaces;

namespace WeaponSystem.Detectors
{
    public abstract class BaseDetector : ScriptableObject
    {
        //TODO: Replace parameters with struct
        public delegate void DetectedAction(IDetectable[] targets, Vector3 hitPoint);
        /// <summary>
        /// Runs detection algorithm, when finished invokes given action with detected targets and point at which detection was started
        /// </summary>
        /// <param name="action"></param>
        public virtual void TryDetect(DetectedAction action)
        {
            Debug.LogWarning($"Not implemented {GetType().Name}. Returning empty Array");
            action.Invoke(Array.Empty<IDetectable>(), Vector3.zero);
        }
    }
}
