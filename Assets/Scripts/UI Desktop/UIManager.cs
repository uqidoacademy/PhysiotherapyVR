using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Desktop;

public class UIManager : MonoBehaviour {

    public GameObject SelectionPatientPanel;

    public GameObject SelectionBodyPartPanel;

    public void ActiveSelectionPatientPanel() { 
        
        SelectionPatientPanel.SetActive(true);
    }
}
