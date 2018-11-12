using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    public class LimbConfiguration {
        public Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();
        public LimbsEnum limb; // set this to understand what articolation name look for
        public float timing;

        /// <summary>
        /// Use this to avoid mispelling when setting arm articolation name
        /// </summary>
        /// <param name="armArticolation">Arm articolation</param>
        public void SetArmArticolation(ArmExerciseStep.ArmArticolationNamesEnum armArticolation, Sensor armSensor)
        {
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(armArticolation), armSensor);
        }
    }
}