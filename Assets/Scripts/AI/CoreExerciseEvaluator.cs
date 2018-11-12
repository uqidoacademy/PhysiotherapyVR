using AI.Error;
using Limb;
using System;
using System.Collections.Generic;
using UnityEngine;

/// <author>
/// Antonio Terpin
/// </author>
namespace AI
{
    public class ExerciseEvaluatorTrainingSet
    {
        public ExerciseStep[] idealMovementSteps;
        public float timing;
    }

    /// <summary>
    /// Class used to perform calculations and retrieve results for the patient exercise execution
    /// </summary>
    public class CoreExerciseEvaluator
    {
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

        public Dictionary<string, ArticolationError> EvaluateExerciseStep(ExerciseStep currentStep)
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

            Dictionary<string, ArticolationError> articolationErrors = new Dictionary<string, ArticolationError>();
            foreach(string articolationName in currentStep.AAT.Keys)
            {
                ArticolationError articolationError = new ArticolationError();
                
                ArticolationPoint currentArticolationPoint = currentStep.AAT[articolationName],
                                  previousArticolationPoint = previousStep != null ? previousStep.AAT[articolationName] : null,
                                  currentIdealArticolationPoint = currentIdealStep.AAT[articolationName],
                                  previousIdealArticolationPoint = previousIdealStep != null ? previousIdealStep.AAT[articolationName] : null;
                
                articolationError.Position.Magnitude = currentIdealArticolationPoint.Position - currentArticolationPoint.Position;
                
                articolationError.Angle.Magnitude = currentIdealArticolationPoint.Angle - currentArticolationPoint.Angle;

                CalculateSpeedErrors(articolationError, currentArticolationPoint, previousArticolationPoint, currentIdealArticolationPoint, previousIdealArticolationPoint);
                
                articolationErrors.Add(articolationName, articolationError);
            }
            return articolationErrors;
        }
    }
}