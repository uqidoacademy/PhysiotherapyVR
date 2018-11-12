using System;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

namespace Limb
{
    public abstract class ExerciseStep
    {
        public ArticolationPoint Root { get; set; }
        public Dictionary<string, ArticolationPoint> AAT = new Dictionary<string, ArticolationPoint>(); // articolation allocation table
    }

    // TODO IN FUTURE GET FROM JSON (quite easy, but left to the reader as a simple exercise :* :* . Email me at antonio.terpin@gmail.com to get more info)
    /* Banalmente il JSON può essere così (da una cosa del genere è semplice costruire un esercizio come successione di exercise step e anche l' AAT):
     * [
     *  {                                                       // articolation type
     *      "position": [#float, #float, #float],               // posizione come vettore
     *      "angle": [#float, #float, #float],                  // angolo come euler angles
     *      "articolationName": #string,                        // nome dell'articolazione, da usare nel dictionary
     *      "substaining": [...#articolationType]
     *  }
     * ]
     */

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
}