using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Configuration of a limb, so describes what the sensors stand for in the real world (e.g. shoulder, elbow, hand, ..)
    /// </summary>
    /// <extension>
    /// To add a limb, extend this class, take as example the ArmConfiguration class
    /// </extension>
    /// <author>Antonio Terpin & Gabriel Ciulei</author>
    public abstract class LimbConfiguration {
        public Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();

        /// <summary>
        /// Take a snapshot of the limb
        /// </summary>
        /// <returns>The snapshot of the limb configuration (transforms of the sensors)</returns>
        public abstract LimbData ExtractLimbData();

        protected Dictionary<string, Transform> GetTransformOutOfSensors()
        {
            Dictionary<string, Transform> transforms = new Dictionary<string, Transform>();
            foreach(string articolationName in sensors.Keys)
            {
                transforms.Add(articolationName, sensors[articolationName].physicalSensor.transform);
            }
            return transforms;
        }
    }

    /// <summary>
    /// Extension of the LimbConfiguration class used to describe an arm
    /// </summary>
    /// <author>Antonio Terpin</author>
    public class ArmConfiguration : LimbConfiguration
    {
        public ArmConfiguration(Sensor shoulder, Sensor elbow, Sensor hand)
        {
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmArticolationNamesEnum.SHOULDER), shoulder);
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmArticolationNamesEnum.ELBOW), elbow);
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmArticolationNamesEnum.HAND), hand);
        }

        public override LimbData ExtractLimbData()
        {
            ArmData data = new ArmData();
            data.articolations = GetTransformOutOfSensors();
            return data;
        }
    }
}