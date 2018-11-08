using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class GetStatus : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		var error = ETrackedPropertyError.TrackedProp_Success;
		for (uint i = 0; i < 16; i++) {
			var result = new System.Text.StringBuilder ((int) 64);
			
			OpenVR.System.GetStringTrackedDeviceProperty (i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);
			if (result.ToString ().Contains ("tracker")) {
				if(OpenVR.System.GetTrackedDeviceActivityLevel(i) == EDeviceActivityLevel.k_EDeviceActivityLevel_Standby)
					Debug.Log("Standby tracker numero: "+i);
			}
		}
	}
}