using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Limb
{
    public abstract class LimbData
    {
        public Dictionary<string, Transform> articolations = new Dictionary<string, Transform>();

        public abstract ExerciseStep UnwrapFromSensors();
    }

    public class ArmData : LimbData
    {
        public override ExerciseStep UnwrapFromSensors()
        {
            return new ArmExerciseStep(
                articolations[ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.SHOULDER)],
                articolations[ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.ELBOW)],
                articolations[ArmExerciseStep.ArmArticolationNameOf(ArmExerciseStep.ArmArticolationNamesEnum.HAND)]
                );
        }
    }
}