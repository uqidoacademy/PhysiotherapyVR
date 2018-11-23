using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class WhatObjectVR : MonoBehaviour {

	// Use this for initialization
	void Start () {
        uint trackerTrovati = 0;
        uint index = 0;
        var error = ETrackedPropertyError.TrackedProp_Success;
        for (uint i = 0; i < 16; i++)
        {
            var result = new System.Text.StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
            
            if (result.ToString().Contains("tracker"))
            {
                Debug.Log(result.ToString());
                trackerTrovati += 1;
                index = i;
                Debug.Log("tracker n "+index);
               // break;
            }
        }
        Debug.Log(trackerTrovati);
    }
	
	// Update is called once per frame
	void Update () {
     
    }
}
