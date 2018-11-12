using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TrackerRenamer : MonoBehaviour {

    int partsRenamed = 0;
    bool canInteract = false;
    private void Start()
    {
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
            if(partsRenamed < StaticTestList.ArtList.Count)
            partsRenamed++;
            EditorUtility.DisplayDialog("Next Tracker", "DEVI ORA SELEZIONARE: " + (StaticTestList.ArtList[partsRenamed]), "OK");
        }

    }

    void CreateCollider()
    {
        this.gameObject.AddComponent<BoxCollider>();
        this.gameObject.AddComponent<Rigidbody>();
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
    }



    //TODO boxcollider ai tracker. Testare che funzioni.
}
