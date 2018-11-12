using System;
using UnityEngine;

namespace Limb
{
    public class ArticolationPoint
    {
        public Vector3 Position { get; set; }
        public Vector3 Angle { get; set; }
        
        public ArticolationPoint AttachedTo { get; set; }
        public ArticolationPoint[] Substaining { get; set; }

        public ArticolationPoint(Vector3 position, Vector3 angle, params ArticolationPoint[] substaining)
        {
            Position = position;
            Angle = angle;
            Substaining = substaining;
            foreach(ArticolationPoint articolation in Substaining)
            {
                articolation.AttachedTo = this;
            }

        }
    }
}