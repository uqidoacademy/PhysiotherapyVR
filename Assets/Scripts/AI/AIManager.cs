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
        // overall results on the full exercise

        public float Score { get; private set; }
        public OverallExerciseResults(float score)
        {
            Score = score;
        }
    }
    public class OverallSessionResults
    {
        // overall results on the full session

        public List<OverallExerciseResults> AllResults { get; private set; }
        public float OverallScore { get; private set; }

        public OverallSessionResults(List<OverallExerciseResults> allResults, float overallScore)
        {
            AllResults = allResults;
            OverallScore = overallScore;
        }
    }
    #endregion

    public interface IAIManager
    {
        // TODO generalize (v2.0)

        #region Arms

        ArmExerciseStep ArmExerciseStepFromSensors(Transform shoulder, Transform elbow, Transform hand);
        void CreateArmSession(ArmExerciseStep[] exerciseSteps, float timing); // inizia una serie di esercizi
        void StartArmExercise(); // inizia una nuova ripetizione
        EvaluationResults EvaluateArmStep(ArmExerciseStep exerciseStep);
        OverallExerciseResults EvaluateArmExercise(); // chiude una ripetizione
        OverallSessionResults CloseArmSession();

        #endregion
    }

    public class AIManager : IAIManager
    {
        public float MAX_SCORE = 10;

        // TODO generalize (v2.0)

        #region Arms
        private Core _armEvaluator;
        private List<OverallExerciseResults> _armExercisesResults = new List<OverallExerciseResults>();
        private List<bool> _armExerciseStepsEvaluation = new List<bool>();

        public Dictionary<string, ArticolationTollerance> ArmTollerance { get; set; }

        public ArmExerciseStep ArmExerciseStepFromSensors(Transform shoulder, Transform elbow, Transform hand)
        {
            elbow = GlobalTransformToRelative(shoulder, elbow);
            hand = GlobalTransformToRelative(elbow, hand);
            return new ArmExerciseStep(shoulder, elbow, hand);
        }

        public OverallSessionResults CloseArmSession()
        {
            if (_armExercisesResults == null || _armExercisesResults.Count <= 0) return null;
            float overallScore = 0;
            foreach(OverallExerciseResults ex in _armExercisesResults) overallScore += ex.Score;
            OverallSessionResults results = new OverallSessionResults(_armExercisesResults, overallScore / _armExercisesResults.Count);
            ClearArmSession();
            return results;
        }

        private void ClearArmSession()
        {
            _armEvaluator = null;
            _armExercisesResults.Clear();
            _armExerciseStepsEvaluation.Clear();
        }

        public void CreateArmSession(ArmExerciseStep[] exerciseSteps, float timing)
        {
            _armEvaluator = new Core(exerciseSteps, timing);
        }

        public EvaluationResults EvaluateArmStep(ArmExerciseStep exerciseStep)
        {
            bool niceWork = true;
            Dictionary<string, ArticolationError> stepEvaluationResults = _armEvaluator.Evaluate(exerciseStep);
            if (ArmTollerance != null)
            {
                // no checks so if there are bugs we get a null pointer exception during test.. I prefere a software with visible bugs
                foreach(string articolationName in ArmTollerance.Keys)
                {
                    ArticolationError error = stepEvaluationResults[articolationName];
                    ArticolationTollerance tollerance = ArmTollerance[articolationName];
                    niceWork &= (error.Position.Speed.magnitude < tollerance.positionSpeedTolleranceRadius)
                        & (error.Position.Value.magnitude < tollerance.positionTolleranceRadius)
                        & (error.Angle.Speed.magnitude < tollerance.rotationSpeedTolleranceRadius)
                        & (error.Angle.Value.magnitude < tollerance.rotationTolleranceRadius);
                }
            }

            return new EvaluationResults(niceWork, stepEvaluationResults);
        }

        public void StartArmExercise()
        {
            _armEvaluator.Restart();
        }

        public OverallExerciseResults EvaluateArmExercise()
        {
            float score = 0;
            foreach(bool res in _armExerciseStepsEvaluation)
            {
                score += res ? 1 : 0;
            }
            OverallExerciseResults results = new OverallExerciseResults(score / _armExerciseStepsEvaluation.Count * MAX_SCORE);
            _armExercisesResults.Add(results);
            return results;
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

        public Transform GlobalTransformToRelative(Transform father, Transform child)
        {
            UtilityTranformGMOne.transform.position = father.position;
            UtilityTranformGMOne.transform.eulerAngles = father.eulerAngles;
            UtilityTranformGMTwo.transform.position = child.position;
            UtilityTranformGMTwo.transform.eulerAngles = child.eulerAngles;

            UtilityTranformGMTwo.transform.SetParent(UtilityTranformGMOne.transform);

            // TODO casomai occhio ai puntatori del ciesso
            return UtilityTranformGMTwo.transform;
        }

        public OverallExerciseResults EvaluateArmExercise(int armExerciseID, int armSessionID)
        {
            throw new System.NotImplementedException();
        }

        #endregion
    }
}