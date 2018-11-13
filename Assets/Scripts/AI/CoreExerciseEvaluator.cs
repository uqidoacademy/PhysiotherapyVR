﻿using AI.Error;
using Limb;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Class used to perform calculations and retrieve results for the patient exercise execution
    /// </summary>
    /// <author>
    /// Antonio Terpin & Gabriel Ciulei & Giovanni Niero
    /// </author>
    public class CoreExerciseEvaluator
    {
        public class ExerciseEvaluatorTrainingSet
        {
            public ExerciseStep[] idealMovementSteps;
            public float timing;
        }

        private ExerciseEvaluatorTrainingSet _trainingSet;

        private List<ExerciseStep> PerformedMovementSteps = new List<ExerciseStep>();

        #region Neural Network initialization

        public CoreExerciseEvaluator(ExerciseEvaluatorTrainingSet trainingSet)
        {
            Init(trainingSet);
        }

        public void Init(ExerciseEvaluatorTrainingSet trainingSet)
        {
            if (IsTrainingSetValid(trainingSet)) throw new ArgumentException("Too few ideal movement steps");
            _trainingSet = trainingSet;
            RestartExercise();
        }

        private bool IsTrainingSetValid(ExerciseEvaluatorTrainingSet trainingSet)
        {
            return trainingSet.idealMovementSteps.Length < 2;
        }

        #endregion

        public void RestartExercise()
        {
            PerformedMovementSteps.Clear();
        }

        #region Utils
        
        private ExerciseStep GetIdealStep(int shiftFromLastPerformedIndex)
        {
            int index = PerformedMovementSteps.Count - 1 + shiftFromLastPerformedIndex;
            if (index >= 0 && index < PerformedMovementSteps.Count)
                return _trainingSet.idealMovementSteps[index];

            return null;
        }
        
        private ExerciseStep GetPerformedStep(int shiftFromLastPerformedIndex)
        {
            int index = PerformedMovementSteps.Count - 1 + shiftFromLastPerformedIndex;
            if (index >= 0 && index < PerformedMovementSteps.Count)
                return PerformedMovementSteps[index];
            return null;
        }
        
        private Vector3 IncrementalRatioOf(Vector3 b, Vector3 a, float time)
        {
            return (b - a) / time;
        }

        private Vector3 CalculateSpeedError(Vector3 idealMagnitude1, Vector3 idealMagnitude2, Vector3 realMagnitude1, Vector3 realMagnitude2, float timing)
        {
            return IncrementalRatioOf(idealMagnitude1, idealMagnitude2, timing) - IncrementalRatioOf(realMagnitude1, realMagnitude2, timing);
        }

        private void CalculateSpeedErrors(ArticolationError articolationError,
        ArticolationPoint currentArticolationPoint, ArticolationPoint previousArticolationPoint,
        ArticolationPoint currentIdealArticolationPoint, ArticolationPoint previousIdealArticolationPoint)
        {
            if (previousIdealArticolationPoint != null && previousArticolationPoint != null)
            {
                articolationError.Position.Speed = CalculateSpeedError(
                    currentIdealArticolationPoint.Position, previousIdealArticolationPoint.Position,
                    currentArticolationPoint.Position, previousArticolationPoint.Position,
                    _trainingSet.timing);

                articolationError.Angle.Speed = CalculateSpeedError(
                    currentIdealArticolationPoint.Angle, previousIdealArticolationPoint.Angle,
                    currentArticolationPoint.Angle, previousArticolationPoint.Angle,
                    _trainingSet.timing);
            }
            else
            {
                articolationError.Position.Speed = Vector3.zero;
                articolationError.Angle.Speed = Vector3.zero;
            }
        }

        #endregion

        public ArticolationError[] EvaluateExerciseStep(ExerciseStep currentStep, ArticolationTollerance[] tollerances)
        {
            PerformedMovementSteps.Add(currentStep);
            ExerciseStep previousStep = GetPerformedStep(-1),
                currentIdealStep = GetIdealStep(0),
                previousIdealStep = GetIdealStep(-1);

            if(currentIdealStep == null)
            {
                int lastStepIndex = _trainingSet.idealMovementSteps.Length - 1;
                currentIdealStep = _trainingSet.idealMovementSteps[lastStepIndex];
                previousIdealStep = _trainingSet.idealMovementSteps[lastStepIndex - 1];
            }

            List<ArticolationError> articolationErrors = new List<ArticolationError>();
            ArticolationPoint currentStepArticolationPoint = currentStep.Root,
                previousStepArticolationPoint = previousStep == null ? null : previousStep.Root,
                currentStepIdealArticolationPoint = currentIdealStep.Root,
                previousStepIdealArticolationPoint = previousIdealStep == null ? null : previousIdealStep.Root;
            int i = 0;
            while(currentStepArticolationPoint != null)
            {
                ArticolationError articolationError = new ArticolationError();
                ArticolationTollerance tollerance = tollerances[i++];

                articolationError.Position.Magnitude = currentStepIdealArticolationPoint.Position - currentStepArticolationPoint.Position;
                articolationError.Position.IsMagnitudeCorrect = articolationError.Position.Magnitude.magnitude < tollerance.positionTolleranceRadius;

                articolationError.Angle.Magnitude = currentStepIdealArticolationPoint.Angle - currentStepArticolationPoint.Angle;
                articolationError.Angle.IsMagnitudeCorrect = articolationError.Angle.Magnitude.magnitude < tollerance.rotationTolleranceRadius;

                CalculateSpeedErrors(articolationError, currentStepArticolationPoint, previousStepArticolationPoint, currentStepIdealArticolationPoint, previousStepIdealArticolationPoint);
                articolationError.Position.IsSpeedCorrect = articolationError.Position.Speed.magnitude < tollerance.positionSpeedTolleranceRadius;
                articolationError.Angle.IsSpeedCorrect = articolationError.Angle.Speed.magnitude < tollerance.positionSpeedTolleranceRadius;

                articolationErrors.Add(articolationError);
                
                currentStepArticolationPoint = currentStepArticolationPoint.Substaining;
                previousStepArticolationPoint = previousStepArticolationPoint == null ? null : previousStepArticolationPoint.Substaining;
                currentStepIdealArticolationPoint = currentStepIdealArticolationPoint.Substaining;
                previousStepIdealArticolationPoint = previousStepIdealArticolationPoint == null ? null : previousStepIdealArticolationPoint.Substaining;
            }
            return articolationErrors.ToArray();
        }
    }
}