using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Generic step of an exercise. An exercise is made by a sequence of this steps.
    /// </summary>
    /// <extension>
    /// To extend this implement an appropriate class on the example made below for the arm. Doing this provide safe names and referral to articulations.
    /// </extension>
    /// <author>Antonio Terpin</author>
    public abstract class ExerciseStep
    {
        public ArticolationPoint Root { get; set; }
        public Dictionary<string, ArticolationPoint> AAT = new Dictionary<string, ArticolationPoint>(); // articolation allocation table
    }

    #region ExerciseStep implementations

    /// <summary>
    /// Example of implementation of ExerciseStep. Describe an Arm exercise step
    /// </summary>
    /// <author>Antonio Terpin</author>
    public class ArmExerciseStep : ExerciseStep
    {
        public static string ArmArticolationNameOf(ArmArticolationNamesEnum articolationName)
        {
            DescriptionAttribute[] attributes = (
                    DescriptionAttribute[]) articolationName
                   .GetType()
                   .GetField(articolationName.ToString())
                   .GetCustomAttributes(typeof(DescriptionAttribute), false
               );
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public ArmExerciseStep(Transform shoulder, Transform elbow, Transform hand)
        {
            ArticolationPoint Hand = new ArticolationPoint(hand.position, hand.eulerAngles);
            ArticolationPoint Elbow = new ArticolationPoint(elbow.position, elbow.eulerAngles, Hand);
            Root = new ArticolationPoint(shoulder.position, shoulder.eulerAngles, Elbow);
            AAT.Add(ArmArticolationNameOf(ArmArticolationNamesEnum.SHOULDER), Root);
            AAT.Add(ArmArticolationNameOf(ArmArticolationNamesEnum.ELBOW), Elbow);
            AAT.Add(ArmArticolationNameOf(ArmArticolationNamesEnum.HAND), Hand);
        }
    }

    #endregion

    #region Articolations names
    /// <summary>
    /// Arm naming
    /// </summary>
    /// <author>Antonio Terpin</author>
    public enum ArmArticolationNamesEnum
    {
        [Description("shoulder")]
        SHOULDER,
        [Description("elbow")]
        ELBOW,
        [Description("hand")]
        HAND
    }

    #endregion
}