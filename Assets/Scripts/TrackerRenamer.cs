using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrackerRenamer : MonoBehaviour {


    private TrackerManager trackerManager;

    int partsRenamed = 0;
    List<string> limbParts;

    private void Start()
    {
        trackerManager = FindObjectOfType<TrackerManager>();
        if (trackerManager == null)
            Debug.LogError("MANCA TRACKER");
        CreateCollider();
    }

    //this function is called by TrackerManager
    public void StartRename(List<string> list)
    {
        limbParts = list;
    }
 
 

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("tracker"))
        {

            other.gameObject.name = limbParts[partsRenamed];
            other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            other.gameObject.GetComponent<MeshRenderer>().material.color = StaticTestList.ColorList[partsRenamed];
            trackerManager.trackerListReady.Add(new TrackerManager.trackerReady() {

                TrackerID = other.gameObject.name,
                reference = other.gameObject,

            });
            UIDesktopManager.I.LimbPartReady(limbParts[partsRenamed]);
            partsRenamed++;

        }

    }

    void CreateCollider()
    {
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
