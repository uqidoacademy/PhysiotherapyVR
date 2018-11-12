using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Limb
{
    /// <summary>
    /// Snapshot of the limb
    /// </summary>
    /// <extension>To have different limbs, extend this class to unwrap the appropriate exercise step</extension>
    /// <author>Antonio Terpin</author>
    public abstract class LimbData
    {
        public Dictionary<string, Transform> articolations = new Dictionary<string, Transform>();

        /// <summary>
        /// Use this to take the snapshot and build a generic exercise step
        /// </summary>
        /// <returns></returns>
        public abstract ExerciseStep UnwrapFromSensors();
    }

    /// <summary>
    /// Example of an arm extension of the generic abstract class. Describe an arm snapshot.
    /// </summary>
    /// <author>Antonio Terpin</author>
    public class ArmData : LimbData
    {
        public override ExerciseStep UnwrapFromSensors()
        {
            return new ArmExerciseStep(
                articolations[ArmExerciseStep.ArmArticolationNameOf(ArmArticolationNamesEnum.SHOULDER)],
                articolations[ArmExerciseStep.ArmArticolationNameOf(ArmArticolationNamesEnum.ELBOW)],
                articolations[ArmExerciseStep.ArmArticolationNameOf(ArmArticolationNamesEnum.HAND)]
                );
        }
    }
}