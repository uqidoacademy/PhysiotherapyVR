using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Describe the tollerance that is given to the movement of an articolation
    /// </summary>
    /// <author>Antonio Terpin & Gabriel Ciulei & Giovanni Niero</author>
    [CreateAssetMenu(fileName = "Data", menuName = "Body/ArticolationTollerance", order = 1)]
    public class ArticolationTollerance : ScriptableObject
    {
        public float positionTolleranceRadius;
        public float positionSpeedTolleranceRadius;
        public float rotationTolleranceRadius;
        public float rotationSpeedTolleranceRadius;
    }
}