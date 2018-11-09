using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    [CreateAssetMenu(fileName = "ArticolationTollerance", menuName = "Limb/ArticolationTollerance", order = 2)]
    public class ArticolationTollerance : ScriptableObject
    {
        [SerializeField] public float positionTolleranceRadius;
        [SerializeField] public float positionSpeedTolleranceRadius;
        [SerializeField] public float rotationTolleranceRadius;
        [SerializeField] public float rotationSpeedTolleranceRadius;
    }
}