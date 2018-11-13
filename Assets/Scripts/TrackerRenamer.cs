using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrackerRenamer : MonoBehaviour {


    private TrackerManager trackerManager;

    int partsRenamed = 0;
    bool canInteract = false;

    private void Start()
    {
        trackerManager = FindObjectOfType<TrackerManager>();
        CreateCollider();
    }

    //this function is called by TrackerManager
    public void SetInteraction(bool value)
    {
        canInteract = value;
        if(canInteract)
        EditorUtility.DisplayDialog("Next Tracker", "DEVI ORA SELEZIONARE: " + (StaticTestList.ArtList[partsRenamed]), "OK");
    }
 
 

    void OnTriggerEnter(Collider other)
    {
        if (canInteract && other.gameObject.name.Contains("tracker"))
        {
            Debug.Log("COLPITO " + other.gameObject.name);
            other.gameObject.name = StaticTestList.ArtList[partsRenamed];
            other.gameObject.GetComponent<BoxCollider>().isTrigger = true;
            other.gameObject.GetComponent<MeshRenderer>().material.color = StaticTestList.ColorList[partsRenamed];
            EditorUtility.DisplayDialog("Tracker Configurations", "HAI AGGIUNTO: " + (StaticTestList.ArtList[partsRenamed]), "OK");
            trackerManager.trackerListReady.Add(new TrackerManager.trackerReady() {

                TrackerID = other.gameObject.name,
                reference = other.gameObject,

            });
            partsRenamed++;

            if (partsRenamed < StaticTestList.ArtList.Count)
            {
                EditorUtility.DisplayDialog("Next Tracker", "DEVI ORA SELEZIONARE: " + (StaticTestList.ArtList[partsRenamed]), "OK");
            }
            else {
                //Ready to record
            }
        }

    }

    void CreateCollider()
    {
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
    }
}
