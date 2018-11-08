using System;
using UnityEngine;

namespace AI
{
    /// <summary>
    /// Descrittore di un'articolazione, con posizione e angolazione.
    /// </summary>
    public class ArticolationPoint
    {
        public Vector3 Position { get; set; }
        public Vector3 Angle { get; set; }
        
        public ArticolationPoint AttachedTo { get; set; }
        public ArticolationPoint[] Substaining { get; set; }

        /// <summary>
        /// Costruttorre di un'articolazione
        /// </summary>
        /// <param name="position">La posizione dell'articolazione relative ad AttachedTo</param>
        /// <param name="angle">L'angolazione dell'articolazione relative ad AttachedTo</param>
        /// <param name="substaining">Le articolazioni (eventuali) che sono sostenute da questa (avranno questa come AttachedTo).</param>
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