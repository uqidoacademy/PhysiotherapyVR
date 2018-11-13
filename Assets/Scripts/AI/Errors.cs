using System;
using UnityEngine;

/// <summary>
/// Describe the errors format
/// </summary>
/// <author>Antonio Terpin & Gabriel Ciulei & Giovanni Niero</author>
namespace AI.Error
{
    /// <summary>
    /// A movement error (both linear and rotational) is composed by the magnitude (e.g. i'm at 1,1,1 instead of 0,0,0) and a speed (i moved to the right position but to fast)
    /// </summary>
    public class MovementError
    {
        public Vector3 Magnitude { get; set; }
        public Vector3 Speed { get; set; }
    }

    /// <summary>
    /// An articolation error is divided in two categories: position and angle
    /// </summary>
    public class ArticolationError
    {
        public MovementError Position = new MovementError();
        public MovementError Angle = new MovementError();
    }
}