using AI.Error;
using Limb;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    #region Results types
    public class EvaluationResults
    {
        public bool NiceWork { get; private set; }
        public Dictionary<string, ArticolationError> Corrections { get; private set; }
        public EvaluationResults(bool niceWork, Dictionary<string, ArticolationError> corrections)
        {
            NiceWork = niceWork;
            Corrections = corrections;
        }
    }
    public class OverallExerciseResults
    {
        public float Score { get; private set; }
        public OverallExerciseResults(float score)
        {
            Score = score;
        }
    }
    public class OverallSessionResults
    {
        public List<OverallExerciseResults> AllResults { get; private set; }
        public float OverallScore { get; private set; }

        public OverallSessionResults(List<OverallExerciseResults> allResults, float overallScore)
        {
            AllResults = allResults;
            OverallScore = overallScore;
        }
    }
    #endregion

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
        public Dictionary<string, ArticolationTollerance> ExerciseTollerance { get; set; }

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

        private void ClearExerciseSession()
        {
            _exerciseEvaluator = null;
            _exercisesResults.Clear();
            _exerciseStepsEvaluation.Clear();
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
            Dictionary<string, ArticolationError> stepEvaluationResults = _exerciseEvaluator.EvaluateExerciseStep(exerciseStep);
            if (ExerciseTollerance != null)
            {
                foreach(string articolationName in ExerciseTollerance.Keys)
                {
                    ArticolationError error = stepEvaluationResults[articolationName];
                    ArticolationTollerance tollerance = ExerciseTollerance[articolationName];
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

        #region Convertions

        private GameObject _utilityTranformGMOne = null;
        private GameObject UtilityTranformGMOne
        {
            get
            {
                if (_utilityTranformGMOne == null)
                {
                    _utilityTranformGMOne = new GameObject();
                    _utilityTranformGMOne.SetActive(false);
                }
                return _utilityTranformGMOne;
            }
        }

        private GameObject _utilityTranformGMTwo = null;
        private GameObject UtilityTranformGMTwo
        {
            get
            {
                if (_utilityTranformGMTwo == null)
                {
                    _utilityTranformGMTwo = new GameObject();
                    _utilityTranformGMTwo.SetActive(false);
                }
                return _utilityTranformGMTwo;
            }
        }

        private Transform GlobalTransformToRelative(Transform father, Transform child)
        {
            UtilityTranformGMOne.transform.position = father.position;
            UtilityTranformGMOne.transform.eulerAngles = father.eulerAngles;
            UtilityTranformGMTwo.transform.position = child.position;
            UtilityTranformGMTwo.transform.eulerAngles = child.eulerAngles;

            UtilityTranformGMTwo.transform.SetParent(UtilityTranformGMOne.transform);

            // TODO casomai occhio ai puntatori del ciesso
            return UtilityTranformGMTwo.transform;
        }

        #endregion
    }
}