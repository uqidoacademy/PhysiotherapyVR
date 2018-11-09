using Physiotherapy.StateMachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UI.Desktop;

public class AppFlowManager : MonoBehaviour
{

    public Text DebugText;

    MyStateMachine sm;

    List<IState> states;

    void Start()
    {
        // Setup SM
        sm = gameObject.AddComponent<MyStateMachine>();

        AppFlowContext context = new AppFlowContext()
        {
            DebugText = DebugText,
            PatientSelectionDoneCallback = patientSelectionDone,
            DoneCallBack = goToNextState,
           // ChooseBodyPartsDoneCallback = patientSelectionToDo,
        };

        states = new List<IState>() {
            new PatientSelection().Setup(context),
            new ChooseBodyParts().Setup(context),
            new WearTracker().Setup(context),
            new SetUpTracker().Setup(context),
            new RegistrationSample().Setup(context),
            new ExerciseReady().Setup(context),
            new DoingExercise().Setup(context),
            new EndExercise().Setup(context),

        };
        sm.Setup(context, states);

        patientSelectionToDo();
    }

    private void goToNextState()
    {
        for (int i = 0; i < states.Count ; i++)
        {
            if (states[i] == sm.CurrentState )
            {
                if (i +1 == states.Count)
                    sm.CurrentState = states[0];
                else
                    sm.CurrentState = states[i + 1];
                break;
            }
            
        }
    }

    #region routes

    private void patientSelectionToDo() {
        Debug.Log("First State");
        sm.CurrentState = sm.States[0];
    }

    private void patientSelectionDone()
    {

        Debug.Log("Second State");
        sm.CurrentState = sm.States[1];
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
    public List<PatientProfile> ListPatient;

    public Text DebugText;

    /// <summary>
    /// Contiene la callBack da richiamare quando è finita la selezione paziente
    /// </summary>
    public Action PatientSelectionDoneCallback;

    /// <summary>
    /// Contiene la callback da richiamare quando è stata fatta la scelta delle parti del corpo
    /// </summary>
    public Action ChooseBodyPartsDoneCallback;

    /// <summary>
    /// Callback generica da richiamare quando finisco il lavoro dello stato
    /// </summary>
    public Action DoneCallBack;

    public Text GetDebugText()
    {
        return DebugText;
    }
}