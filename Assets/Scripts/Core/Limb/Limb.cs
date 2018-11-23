using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Limb
{
    /// <summary>
    /// Snapshot of the limb
    /// </summary>
    /// <author>Antonio Terpin</author>
    public class LimbData
    {
        public Transform[] articolations;

        /// <summary>
        /// Use this to take the snapshot and build a generic exercise step
        /// </summary>
        /// <returns></returns>
        public ExerciseStep UnwrapFromSensors()
        {
            return new ExerciseStep(articolations);
        }
    }
}