using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackerRenamer : MonoBehaviour {

    List<string> nomi = new List<string>();

    private void Start()
    {
        nomi[0] = "Spalla";
        nomi[1] = "Gomito";
        nomi[2] = "Mano";
    }

    //StaticTestList.ArtsList

    public void SetInteraction(bool value)
    {



    }

    void OnTriggerEnter(Collider other)
    {
    
      
        for (int i = 0; i < nomi.Count; i++)
        {
            other.gameObject.name = nomi[i];
        }

    }

}
