using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using AI.Error;

/// <summary>
/// Results provided by AI manager
/// </summary>
/// <author>Antonio Terpin & Gabriel Ciulei</author>
namespace AI.Results
{
    public class EvaluationResults
    {
        public bool NiceWork { get; private set; }
        public ArticolationError[] Corrections { get; private set; }
        public EvaluationResults(bool niceWork, ArticolationError[] corrections)
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
}