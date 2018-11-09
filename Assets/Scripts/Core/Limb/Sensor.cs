using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    [CreateAssetMenu(fileName = "Sensor", menuName = "Limb/Sensor", order = 1)]
    public class Sensor : ScriptableObject
    {
        [SerializeField] public GameObject physicalSensor;
        [SerializeField] public ArticolationTollerance sensorTollerance;
    }
}
