using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Limb
{
    public class Sensor : ScriptableObject
    {
        [SerializeField] public GameObject physicalSensor;
        [SerializeField] public ArticolationTollerance sensorTollerance;
    }

    public class ArticolationTollerance : ScriptableObject
    {
        [SerializeField] public float PositionTolleranceRadius { get; set; }
        [SerializeField] public float PositionSpeedTolleranceRadius { get; set; }
        [SerializeField] public float RotationTolleranceRadius { get; set; }
        [SerializeField] public float RotationSpeedTolleranceRadius { get; set; }
    }

    public class LimbConfiguration : ScriptableObject
    {
        [SerializeField] public Dictionary<string, Sensor> sensors;
        [SerializeField] public LimbsEnum limb; // set this to understand what articolation name look for
        [SerializeField] public float timing;

        /// <summary>
        /// Use this to avoid mispelling when setting arm articolation name
        /// </summary>
        /// <param name="armArticolation">Arm articolation</param>
        public void SetArmArticolation(ArmExerciseStep.ArmArticolationNamesEnum armArticolation, Sensor armSensor)
        {
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(armArticolation), armSensor);
        }
    }

    public class LimbData
    {
        public Dictionary<string, Transform> articolations;
        public LimbsEnum limb;
    }

    public enum LimbsEnum
    {
        ARM
    }
}