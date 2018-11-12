using System;
using UnityEngine;

namespace AI.Error
{
    public class MovementError
    {
        public Vector3 Magnitude { get; set; }
        public Vector3 Speed { get; set; }
    }

    public class ArticolationError
    {
        public MovementError Position = new MovementError();
        public MovementError Angle = new MovementError();
    }
}