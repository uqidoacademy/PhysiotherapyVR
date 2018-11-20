using AI.Error;
using AI.Results;
using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Wrap the ai core to return well formatted results through its api
    /// </summary>
    /// <author>Antonio Terpin</author>
    public class AIManager
    {
        public float MAX_SCORE = 10;
        
        private CoreExerciseEvaluator _exerciseEvaluator;
        private List<OverallExerciseResults> _exercisesResults = new List<OverallExerciseResults>();
        private List<bool> _exerciseStepsEvaluation = new List<bool>();

        /// <summary>
        /// Use this to set exercise tollerance for all the articulations
        /// </summary>
        public ArticolationTollerance[] ExerciseTollerance { get; set; }

        private void ClearExerciseSession()
        {
            _exerciseEvaluator = null;
            _exercisesResults.Clear();
            _exerciseStepsEvaluation.Clear();
        }

        #region API

        public OverallSessionResults CloseExerciseSession()
        {
            if (_exercisesResults == null || _exercisesResults.Count <= 0) return null;
            float overallScore = 0;
            foreach(OverallExerciseResults ex in _exercisesResults) overallScore += ex.Score;
            OverallSessionResults results = new OverallSessionResults(_exercisesResults, overallScore / _exercisesResults.Count);
            ClearExerciseSession();
            return results;
        }

        /// <summary>
        /// Create a new exercise session (made of multiple repetitions potentially)
        /// </summary>
        /// <param name="timing">timing between each sample</param>
        /// <param name="exerciseSteps">if provided represents the ideal steps, if not real time ideal sampling from a ghost is needed</param>
        public void CreateExerciseSession(float timing, ExerciseStep[] exerciseSteps = null)
        {
            CoreExerciseEvaluator.ExerciseEvaluatorTrainingSet trainingSet = new CoreExerciseEvaluator.ExerciseEvaluatorTrainingSet();
            if (exerciseSteps == null) trainingSet.idealMovementSteps = null;
            else
            {
                List<ExerciseStep> exerciseStepsList = new List<ExerciseStep>();
                exerciseStepsList.AddRange(exerciseSteps);
                trainingSet.idealMovementSteps = exerciseStepsList;
            }
            trainingSet.timing = timing;
            _exerciseEvaluator = new CoreExerciseEvaluator(trainingSet);
        }

        /// <summary>
        /// Evaluate an exercise step
        /// </summary>
        /// <param name="exerciseStep">The performed step</param>
        /// <param name="idealExerciseStep">Optional, required if no training set was provided</param>
        /// <returns></returns>
        public EvaluationResults EvaluateExerciseStep(ExerciseStep exerciseStep, ExerciseStep idealExerciseStep = null)
        {
            bool niceWork = true;
            ArticolationError[] stepEvaluationResults = _exerciseEvaluator.EvaluateExerciseStep(ExerciseTollerance, exerciseStep, idealExerciseStep);
            if (ExerciseTollerance != null)
            {
                for(int i = 0; i < stepEvaluationResults.Length; i++)
                {
                    ArticolationError error = stepEvaluationResults[i];
                    niceWork &= error.Position.IsMagnitudeCorrect && error.Position.IsSpeedCorrect
                        && error.Angle.IsSpeedCorrect && error.Angle.IsMagnitudeCorrect;
                }
            }

            return new EvaluationResults(niceWork, stepEvaluationResults);
        }

        public void StartExercise(bool isRealTimeSampling = false)
        {
            _exerciseEvaluator.RestartExercise();
        }

        public OverallExerciseResults EvaluateExercise()
        {
            float score = 0;
            foreach(bool res in _exerciseStepsEvaluation)
            {
                score += res ? 1 : 0;
            }
            OverallExerciseResults results = new OverallExerciseResults(score / _exerciseStepsEvaluation.Count * MAX_SCORE);
            _exercisesResults.Add(results);
            return results;
        }

        #endregion
    }
}