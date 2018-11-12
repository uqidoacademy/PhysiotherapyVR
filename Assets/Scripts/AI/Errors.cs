using System;
using UnityEngine;

namespace AI.Error
{
    /// <summary>
    /// Generic error (can be position or angolation)
    /// </summary>
    public class MovementError
    {
        public Vector3 Magnitude { get; set; }
        public Vector3 Speed { get; set; }
    }

    /// <summary>
    /// Articolation error are made by to movement errors: angolation and position
    /// </summary>
    public class ArticolationError
    {
        public MovementError Position = new MovementError();
        public MovementError Angle = new MovementError();
    }
}