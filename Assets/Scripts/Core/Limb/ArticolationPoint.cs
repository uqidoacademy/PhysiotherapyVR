using System;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Describe how an articolation point is made:
    /// 1) an articolation has a position and an angle
    /// 2) an articolation may be attached to another (single) articulation
    /// 3) an articolation may substain many other articulations
    /// </summary>
    /// <author>Antonio Terpin & Gabriel Ciulei & Giovanni Niero</author>
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