using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Limb
{
    public class LimbData
    {
        public Dictionary<string, Transform> articolations;
        public LimbsEnum limb;
    }

    public enum LimbsEnum
    {
        ARM
    }
}