using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace AI
{
    public class ExerciseStep
    {
        public ArticolationPoint Root { get; set; }
        public Dictionary<string, ArticolationPoint> AAT = new Dictionary<string, ArticolationPoint>(); // articolation allocation table
    }

    // TODO IN FUTURE GET FROM JSON

    /// <summary>
    /// Un braccio è identificato dalle sue articolazioni, cioè la spalla, il gomito e il polso.
    /// Ogni punto ha una posizione (quella del gomito relativa alla spalla, quella del polso relativa al gomito) e un'angolazione, anch'essa relativa.
    /// </summary>
    public class ArmExerciseStep : ExerciseStep
    {
        public enum ArmArticolationNamesEnum
        {
            [Description("shoulder")]
            SHOULDER,
            [Description("elbow")]
            ELBOW,
            [Description("hand")]
            HAND
        }

        private static string ArmArticolationNameOf(ArmArticolationNamesEnum articolationName)
        {
            DescriptionAttribute[] attributes = (
                    DescriptionAttribute[]) articolationName
                   .GetType()
                   .GetField(articolationName.ToString())
                   .GetCustomAttributes(typeof(DescriptionAttribute), false
               );
            return attributes.Length > 0 ? attributes[0].Description : string.Empty;
        }

        public ArmExerciseStep(ArticolationPoint shoulder, ArticolationPoint elbow, ArticolationPoint hand, bool alreadyRelative = false)
        {
            ArticolationPoint Hand = new ArticolationPoint(hand.Position, hand.Angle, alreadyRelative);
            ArticolationPoint Elbow = new ArticolationPoint(elbow.Position, elbow.Angle, alreadyRelative, Hand);
            Root = new ArticolationPoint(shoulder.Position, shoulder.Angle, alreadyRelative, Elbow);
            AAT.Add(ArmArticolationNameOf(ArmArticolationNamesEnum.SHOULDER), Root);
            AAT.Add(ArmArticolationNameOf(ArmArticolationNamesEnum.ELBOW), Elbow);
            AAT.Add(ArmArticolationNameOf(ArmArticolationNamesEnum.HAND), Hand);
        }
    }
}