using Physiotherapy.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AppFlowManager : MonoBehaviour
{

    MyStateMachine sm;

    void Start()
    {
        // Setup SM
        sm = gameObject.AddComponent<MyStateMachine>();

        AppFlowContext context = new AppFlowContext()
        {
            
            PatientSelectionDoneCallback = patientSelectionDone,
        };
        List<IState> states = new List<IState>() {
            new PatientSelection().Setup(context),
        };

        sm.Setup(context, states);

        patientSelectionToDo();
    }

    #region routes

    private void patientSelectionToDo() {
        Debug.Log("First State");
        sm.CurrentState = sm.States[0];
    }

    private void patientSelectionDone()
    {
        Debug.Log("Second State");
        //sm.CurrentState = sm.States[1];
    }

    #endregion

    //void OnStart()
    //{
    //    sm.CurrentState = sm.States[0];
    //}

    //void OnSetupDone()
    //{
    //    sm.CurrentState = sm.States[1];
    //}
}


public struct AppFlowContext : IContext
{
    /// <summary>
    /// Contiene la callBack da richiamare quando è finita la selezione paziente
    /// </summary>
    public Action PatientSelectionDoneCallback;
}