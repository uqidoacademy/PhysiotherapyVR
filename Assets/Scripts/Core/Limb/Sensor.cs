using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Used to describe a sensor assigned to an articulation
    /// </summary>
    /// <author>Antonio Terpin & Gabriel Ciulei</author>
    public class Sensor
    {
        public GameObject physicalSensor;
        public ArticolationTollerance sensorTollerance;
        public string sensorID;

        public Sensor(GameObject physicalSensor, ArticolationTollerance sensorTollerance, string sensorID)
        {
            this.physicalSensor = physicalSensor;
            this.sensorTollerance = sensorTollerance;
            this.sensorID = sensorID;
        }
    }
}
