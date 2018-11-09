using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class TrackerManager : MonoBehaviour {

    public GameObject gameObjectTemplate;
	// Use this for initialization
	void Start () {

        InstantiateTrackers(FindTrackerIndex());

    }

    List<uint> FindTrackerIndex()
    {
        List<uint> trackerIndex = new List<uint>();
        var error = ETrackedPropertyError.TrackedProp_Success;
        for (uint i = 0; i < 16; i++)
        {
            var result = new System.Text.StringBuilder((int)64);
            OpenVR.System.GetStringTrackedDeviceProperty(i, ETrackedDeviceProperty.Prop_RenderModelName_String, result, 64, ref error);

            if (result.ToString().Contains("tracker"))
            {
                trackerIndex.Add(i);
               
            }
        }

        return trackerIndex;

    }


  //instantiate game objects based on how many indexes there are in the indexList

  void InstantiateTrackers(List<uint> indexList)
    {
        foreach(uint index in indexList)
        {
            GameObject childTracker = Instantiate(gameObjectTemplate, transform);
            childTracker.name = "tracker_" + index;
            childTracker.AddComponent<SteamVR_TrackedObject>().SetDeviceIndex((int) index);
        }

    }

  void TrackersCount()
  {
    if (StaticTestList.ArtList.Count == FindTrackerIndex().Count)
    {
      TrackerRenamer trackerRenamer = new TrackerRenamer();
      trackerRenamer.SetInteraction(true); //script da aggiunggere al controller
      
    }
    else
    {
      Debug.LogWarning("There are " + (StaticTestList.ArtList.Count - FindTrackerIndex().Count) + " not initialized");

    }
  }

}
