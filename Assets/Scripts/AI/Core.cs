using AI.Error;
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

        #region Initialization

        public Core(ExerciseStep[] idealMovementSteps, float timing)
        {
            Init(idealMovementSteps, timing);
        }

        public void Init(ExerciseStep[] idealMovementSteps, float timing)
        {
            _idealMovementSteps = idealMovementSteps; // ricomincia l'esercizio
            _timing = timing;
            Restart();
        }

        public void Restart()
        {
            PerformedMovementSteps.Clear();
        }

        #endregion

        #region Utils

        private ExerciseStep GetIdealStep(int shift)
        {
            int index = PerformedMovementSteps.Count - 1 + shift;
            if (index >= 0 && index < PerformedMovementSteps.Count)
                return _idealMovementSteps[index];

            return null;
        }

        private ExerciseStep GetPerformedStep(int shift)
        {
            int index = PerformedMovementSteps.Count - 1 + shift;
            if (index >= 0 && index < PerformedMovementSteps.Count)
                return PerformedMovementSteps[index];
            return null;
        }

        private Vector3 DerivativeOf(Vector3 b, Vector3 a, float time)
        {
            return (b - a) / time;
        }

        #endregion

        public Dictionary<string, ArticolationError> Evaluate(ExerciseStep currentStep)
        {
            PerformedMovementSteps.Add(currentStep);
            ExerciseStep previousStep = GetPerformedStep(-1),
                currentIdealStep = GetIdealStep(0),
                previousIdealStep = GetIdealStep(-1);
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
                    articolationError.Position.Speed = DerivativeOf(currentIdealArticolationPoint.Position, previousIdealArticolationPoint.Position, _timing) -
                        DerivativeOf(currentArticolationPoint.Position, previousArticolationPoint.Position, _timing);
                    articolationError.Angle.Speed = DerivativeOf(currentIdealArticolationPoint.Angle, previousIdealArticolationPoint.Angle, _timing) -
                        DerivativeOf(currentArticolationPoint.Angle, previousArticolationPoint.Angle, _timing);
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