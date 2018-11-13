using AI.Error;
using AI.Results;
using System.Collections.Generic;

namespace AI.Proxy
{
    public abstract class AIProxy
    {
        public abstract ArticolationError UnwrapFromResults(string id, EvaluationResults results);

        protected int GetIndexOf(string id, List<string> names)
        {
            return 0;
        }
    }

    public class ArmAIProxy : AIProxy
    {
        public override ArticolationError UnwrapFromResults(string id, EvaluationResults results)
        {
            return results.Corrections[GetIndexOf(id, StaticTestList.ArtList)];
        }
    }
}