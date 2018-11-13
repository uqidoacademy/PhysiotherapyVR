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

        public void CreateExerciseSession(ExerciseStep[] exerciseSteps, float timing)
        {
            CoreExerciseEvaluator.ExerciseEvaluatorTrainingSet trainingSet = new CoreExerciseEvaluator.ExerciseEvaluatorTrainingSet();
            trainingSet.idealMovementSteps = exerciseSteps;
            trainingSet.timing = timing;
            _exerciseEvaluator = new CoreExerciseEvaluator(trainingSet);
        }

        public EvaluationResults EvaluateExerciseStep(ExerciseStep exerciseStep)
        {
            bool niceWork = true;
            ArticolationError[] stepEvaluationResults = _exerciseEvaluator.EvaluateExerciseStep(exerciseStep);
            if (ExerciseTollerance != null)
            {
                for(int i = 0; i < ExerciseTollerance.Length; i++)
                {
                    ArticolationError error = stepEvaluationResults[i];
                    ArticolationTollerance tollerance = ExerciseTollerance[i];
                    niceWork &= (error.Position.Speed.magnitude < tollerance.positionSpeedTolleranceRadius)
                        & (error.Position.Magnitude.magnitude < tollerance.positionTolleranceRadius)
                        & (error.Angle.Speed.magnitude < tollerance.rotationSpeedTolleranceRadius)
                        & (error.Angle.Magnitude.magnitude < tollerance.rotationTolleranceRadius);
                }
            }

            return new EvaluationResults(niceWork, stepEvaluationResults);
        }

        public void StartExercise()
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

        public OverallExerciseResults EvaluateArmExercise(int armExerciseID, int armSessionID)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}