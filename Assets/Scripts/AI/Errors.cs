using System;
using UnityEngine;

namespace AI.Error
{
    public class MovementError
    {
        public Vector3 Value { get; set; }
        public Vector3 Speed { get; set; }
    }

    public class ArticolationError
    {
        public MovementError Position { get; set; }
        public MovementError Angle { get; set; }
    }
}