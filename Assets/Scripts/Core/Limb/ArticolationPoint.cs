using System;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Describe how an articolation point is made:
    /// 1) an articolation has a position and an angle
    /// 2) an articolation may be attached to another (single) articulation
    /// 3) an articolation may substain only one other articulation (if you need multiple substainings, then consider it as a multiple limbs composition)
    /// </summary>
    /// <author>Antonio Terpin & Gabriel Ciulei & Giovanni Niero</author>
    public class ArticolationPoint
    {
        private Transform _articulationTransform;
        public Vector3 Position { get; set; }
        public Vector3 Angle { get; set; }

        private ArticolationPoint _substaining;
        public ArticolationPoint Substaining {
            get
            {
                return _substaining;
            }
            set
            {
                // TODO transform position and angle to relative
                _substaining = value;
            }
        }

        public ArticolationPoint(Vector3 position, Vector3 angle)
        {
            Position = position;
            Angle = angle;
        }
    }
}