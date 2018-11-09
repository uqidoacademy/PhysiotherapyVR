using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerRenamer : MonoBehaviour {

    bool canInteract = false;

    //this function is called by TrackerManager
    public void SetInteraction(bool value)
    {
        canInteract = value;

        if (canInteract)
            CreateCollider();
    }

    void OnTriggerEnter(Collider other)
    {

        if (canInteract)
        {
            for (int i = 0; i < StaticTestList.ArtList.Count; i++)
            {
                other.gameObject.name = StaticTestList.ArtList[i];
            }
            canInteract = false;
        }

    }

    void CreateCollider()
    {
        this.gameObject.AddComponent<BoxCollider>().isTrigger = true;
        this.gameObject.GetComponent<BoxCollider>().size = new Vector3(0.1f, 0.1f, 0.1f);
    }


    //TODO boxcollider ai tracker. Testare che funzioni.
}
