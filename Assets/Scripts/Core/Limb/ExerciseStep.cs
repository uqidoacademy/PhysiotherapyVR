using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Generic step of an exercise. An exercise is made by a sequence of this steps.
    /// </summary>
    /// <author>Antonio Terpin & Gabriel Ciulei</author>
    public class ExerciseStep
    {
        public ArticolationPoint Root { get; set; }

        public ExerciseStep(params Transform[] articulations)
        {
            if (articulations.Length == 0) throw new NullReferenceException("Exercise step without sensors");
            ArticolationPoint[] articulationsPoints = new ArticolationPoint[articulations.Length];
            for(int i = 0; i < articulations.Length; i++)
            {
                Transform articulation = articulations[i];
                articulationsPoints[i] = new ArticolationPoint(articulation.position, articulation.eulerAngles);
            }
            for(int i = 0; i < articulationsPoints.Length - 1; i++)
            {
                articulationsPoints[i].Substaining = articulationsPoints[i + 1];
            }
            Root = articulationsPoints[0];
        }
    }
}