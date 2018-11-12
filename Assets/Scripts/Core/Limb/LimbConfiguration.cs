using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    public abstract class LimbConfiguration {
        public Dictionary<string, Sensor> sensors = new Dictionary<string, Sensor>();

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

    public class ArmConfiguration : LimbConfiguration
    {
        public ArmConfiguration(Sensor shoulder, Sensor elbow, Sensor hand)
        {
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.SHOULDER), shoulder);
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.ELBOW), elbow);
            sensors.Add(ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.HAND), hand);
        }

        public override LimbData ExtractLimbData()
        {
            ArmData data = new ArmData();
            data.articolations = GetTransformOutOfSensors();
            return data;
        }
    }
}