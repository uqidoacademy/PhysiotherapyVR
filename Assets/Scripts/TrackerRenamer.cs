using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerRenamer : MonoBehaviour {

    bool canInteract = false;

    public void SetInteraction(bool value)
    {
        canInteract = value;
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

}
