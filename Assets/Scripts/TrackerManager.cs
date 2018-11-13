using Physiotherapy.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Valve.VR;

public class TrackerManager : MonoBehaviour
{

    public List<trackerReady> trackerListReady;

    public bool setUpTrackerDone = false;

    public void SetUpTrackers()
    {
        TrackersCount();
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
        foreach (uint index in indexList)
        {
            GameObject childTracker = Instantiate(GameObject.CreatePrimitive(PrimitiveType.Cube), transform);
            childTracker.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            childTracker.AddComponent<BoxCollider>().isTrigger = true;
            childTracker.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
            childTracker.name = "tracker_" + index;
            childTracker.AddComponent<SteamVR_TrackedObject>().SetDeviceIndex((int)index);
        }

    }
    //check how many trackers are active based on how many trackers are needed for the exercise
    //if all ok, call trackerRenamer
    //else if some trackers are missing--> display dialog box

    void TrackersCount()
    {

        if (StaticTestList.ArtList.Count == FindTrackerIndex().Count)
        {
            setUpTrackerDone = true;
            //trackerRenamer.SetInteraction(true);
            
        }
        else
        {
            int notInitTrackers = StaticTestList.ArtList.Count - FindTrackerIndex().Count;
            EditorUtility.DisplayDialog("Tracker missing",
              "There are " +
              notInitTrackers +
              (notInitTrackers == 1 ? " tracker not initialized" : " trackers not initialized"),
              "OK");
           // trackerRenamer.SetInteraction(false);

        }
    }

    public struct trackerReady
    {
        public string TrackerID;
        public GameObject reference;

        public bool isReady{ get {
                    return reference;
            } }

    }

}
