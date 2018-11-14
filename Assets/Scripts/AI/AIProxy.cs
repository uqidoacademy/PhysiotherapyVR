using AI.Error;
using AI.Results;
using System.Collections.Generic;

namespace AI.Proxy
{
    public class AIProxy
    {
        public ArticolationError UnwrapFromResults(string id, EvaluationResults results, List<string> limbIds)
        {
            return results.Corrections[GetIndexOf(id, limbIds)];
        }

        protected int GetIndexOf(string id, List<string> names)
        {
            return names.IndexOf(id);
        }
    }
}