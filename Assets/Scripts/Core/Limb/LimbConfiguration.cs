using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    [CreateAssetMenu(fileName = "LimbConfiguration", menuName = "Limb/LimbConfiguration", order = 3)]
    public class LimbConfiguration : ScriptableObject
    {
        public Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();
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

        // to use this from unity editor (because dictionary is not serializable)
        [SerializeField] public bool configuredFromUnityEditor = false;
        [SerializeField] public List<string> sensorsNameFromUnityEditor;
        [SerializeField] public List<Sensor> sensorsFromUnityEditor;
    }
}