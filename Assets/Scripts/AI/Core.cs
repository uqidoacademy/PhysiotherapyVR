using AI.Error;
using Limb;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace AI
{
    public class Core
    {
        private ExerciseStep[] _idealMovementSteps;
        public List<ExerciseStep> PerformedMovementSteps { get; private set; }

        private float _timing;

        /*
         * TODO as another simple exercise for the reader (first is importing from JSON exercise step) implement a function to wrap the ideal movement steps concept, 
         * so if you want to use more ideal movements steps and something like that you can. Email me at antonio.terpin@gmail.com to get more info. :* :*
         */

        #region Initialization

        /// <summary>
        /// Constructor of core (core initialization)
        /// </summary>
        /// <param name="idealMovementSteps">How the exercise should be done</param>
        /// <param name="timing">The time between each step</param>
        public Core(ExerciseStep[] idealMovementSteps, float timing)
        {
            Init(idealMovementSteps, timing);
        }

        /// <summary>
        /// Can be used to initialize again the core object.
        /// </summary>
        /// <param name="idealMovementSteps">How the exercise should be done</param>
        /// <param name="timing">The time between each step</param>
        public void Init(ExerciseStep[] idealMovementSteps, float timing)
        {
            // check ideal movement steps are at least 2
            if (_idealMovementSteps.Length < 2) throw new ArgumentException("Too few ideal movement steps");
            _idealMovementSteps = idealMovementSteps; // ricomincia l'esercizio
            _timing = timing;
            Restart();
        }

        /// <summary>
        /// Used to restart an exercise.
        /// </summary>
        public void Restart()
        {
            PerformedMovementSteps.Clear();
        }

        #endregion

        #region Utils

        // Get the ideal step away from current of shift. Returns null if the step doesn't exist. 
        private ExerciseStep GetIdealStep(int shift)
        {
            int index = PerformedMovementSteps.Count - 1 + shift;
            if (index >= 0 && index < PerformedMovementSteps.Count)
                return _idealMovementSteps[index];

            return null;
        }

        // Get the performed step away from current of shift. Returns null if the step doesn't exist.
        private ExerciseStep GetPerformedStep(int shift)
        {
            int index = PerformedMovementSteps.Count - 1 + shift;
            if (index >= 0 && index < PerformedMovementSteps.Count)
                return PerformedMovementSteps[index];
            return null;
        }

        // calculate the incremental ratio
        private Vector3 IncrementalRatioOf(Vector3 b, Vector3 a, float time)
        {
            return (b - a) / time;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentStep"></param>
        /// <returns></returns>
        public Dictionary<string, ArticolationError> Evaluate(ExerciseStep currentStep)
        {
            PerformedMovementSteps.Add(currentStep);
            ExerciseStep previousStep = GetPerformedStep(-1),
                currentIdealStep = GetIdealStep(0),
                previousIdealStep = GetIdealStep(-1);

            if(currentIdealStep == null)
            {
                // too much real steps !!! speed error
                // TODO handle better maybe
                // at the moment compare with last ideal step always
                currentIdealStep = _idealMovementSteps[_idealMovementSteps.Length - 1];
                previousIdealStep = _idealMovementSteps[_idealMovementSteps.Length - 2];
            }

            Dictionary<string, ArticolationError> articolationErrors = new Dictionary<string, ArticolationError>();
            foreach(string articolationName in currentStep.AAT.Keys)
            {
                ArticolationError articolationError = new ArticolationError();
                // 1. Aggiungo current step alla lista dei movimenti effettuati e prendo il precedente
                ArticolationPoint currentArticolationPoint = currentStep.AAT[articolationName],
                                  previousArticolationPoint = previousStep != null ? previousStep.AAT[articolationName] : null,
                                  currentIdealArticolationPoint = currentIdealStep.AAT[articolationName],
                                  previousIdealArticolationPoint = previousIdealStep != null ? previousIdealStep.AAT[articolationName] : null;
                // 2. Posizione
                articolationError.Position.Value = currentIdealArticolationPoint.Position - currentArticolationPoint.Position;
                // 3. Angolazione
                articolationError.Angle.Value = currentIdealArticolationPoint.Angle - currentArticolationPoint.Angle;
                // 2/3. Speed
                if (previousIdealArticolationPoint != null && previousArticolationPoint != null)
                {
                    articolationError.Position.Speed = IncrementalRatioOf(currentIdealArticolationPoint.Position, previousIdealArticolationPoint.Position, _timing) -
                        IncrementalRatioOf(currentArticolationPoint.Position, previousArticolationPoint.Position, _timing);
                    articolationError.Angle.Speed = IncrementalRatioOf(currentIdealArticolationPoint.Angle, previousIdealArticolationPoint.Angle, _timing) -
                        IncrementalRatioOf(currentArticolationPoint.Angle, previousArticolationPoint.Angle, _timing);
                } else
                {
                    articolationError.Position.Speed = Vector3.zero;
                    articolationError.Angle.Speed = Vector3.zero;
                }
                // 4. Memorizzo l'errore
                articolationErrors.Add(articolationName, articolationError);
            }
            return articolationErrors;
        }
    }
}