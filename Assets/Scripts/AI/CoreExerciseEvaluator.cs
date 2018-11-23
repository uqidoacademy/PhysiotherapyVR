using AI.Error;
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
            public List<ExerciseStep> idealMovementSteps;
            public float timing;
        }

        private ExerciseEvaluatorTrainingSet _trainingSet;
        private bool _isRealTimeSampling = false;

        private List<ExerciseStep> PerformedMovementSteps = new List<ExerciseStep>();

        #region Neural Network initialization

        public CoreExerciseEvaluator(ExerciseEvaluatorTrainingSet trainingSet)
        {
            Init(trainingSet);
        }

        public void Init(ExerciseEvaluatorTrainingSet trainingSet)
        {
            if (!IsTrainingSetValid(trainingSet)) throw new ArgumentException("Too few ideal movement steps");
            if(trainingSet.idealMovementSteps == null)
            {
                _isRealTimeSampling = true;
                trainingSet.idealMovementSteps = new List<ExerciseStep>();
            }
            _trainingSet = trainingSet;
            

            RestartExercise();
        }

        private bool IsTrainingSetValid(ExerciseEvaluatorTrainingSet trainingSet)
        {
            // the training set is valid if is null (real time sampling) or if it is at minimum two sample long
            return trainingSet.idealMovementSteps == null || trainingSet.idealMovementSteps.Count >= 2;
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
            if (index >= 0 && index < _trainingSet.idealMovementSteps.Count)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tollerances"></param>
        /// <param name="currentStep"></param>
        /// <param name="idealExerciseStep"></param>
        /// <returns></returns>
        public ArticolationError[] EvaluateExerciseStep(ArticolationTollerance[] tollerances, ExerciseStep currentStep, ExerciseStep idealExerciseStep = null)
        {
            // check abiity to evaluate the step
            if (_isRealTimeSampling && idealExerciseStep == null) throw new Exception("Unable to evaluate exercise step without ideal sample or a training set!");

            PerformedMovementSteps.Add(currentStep);

            // eventually keep training the ai (doing this all the following code is the same in both cases)
            if(idealExerciseStep != null) _trainingSet.idealMovementSteps.Add(idealExerciseStep);
            
            ExerciseStep previousStep = GetPerformedStep(-1),
                currentIdealStep = GetIdealStep(0),
                previousIdealStep = GetIdealStep(-1);

            if(currentIdealStep == null)
            {
                int lastStepIndex = _trainingSet.idealMovementSteps.Count - 1;
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

                Debug.Log("Articolation [" + i + "]");
                Debug.Log("ideal position (" +
                    currentStepIdealArticolationPoint.Position.x + ", " + currentStepIdealArticolationPoint.Position.y + ", " + currentStepIdealArticolationPoint.Position.z + ")"
                    );
                Debug.Log("real position (" +
                    currentStepArticolationPoint.Position.x + ", " + currentStepArticolationPoint.Position.y + ", " + currentStepArticolationPoint.Position.z + ")"
                    );

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