using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Limb
{
    /// <summary>
    /// Configuration of a limb, so describes what the sensors stand for in the real world (e.g. shoulder, elbow, hand, ..)
    /// </summary>
    /// <author>Antonio Terpin & Gabriel Ciulei</author>
    public class LimbConfiguration {
        public Sensor[] sensors;

        /// <summary>
        /// Take a snapshot of the limb
        /// </summary>
        /// <returns>The snapshot of the limb configuration (transforms of the sensors)</returns>
        public LimbData ExtractLimbData()
        {
            LimbData limbData = new LimbData();
            limbData.articolations = GetTransformOutOfSensors();
            return limbData;
        }

        protected Transform[] GetTransformOutOfSensors()
        {
            Transform[] transforms = new Transform[sensors.Length];
            for(int i = 0; i < sensors.Length; i++)
            {
                transforms[i] = sensors[i].physicalSensor.transform;
            }
            return transforms;
        }

        public LimbConfiguration(params Sensor[] sensors)
        {
            this.sensors = sensors;
        }
    }
}