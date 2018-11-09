using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class PatientSelection : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {
            Debug.LogFormat("State {0} loaded.", "GameplayState");
            myContext = (AppFlowContext)context;



        }

        
        public override void Tick()
        {
            Debug.LogFormat("Setup state {0} tick.", "SetupState");
            if (Input.GetKeyDown(KeyCode.A))
            {
                myContext.PatientSelectionDoneCallback();
            }
            //myContext.SetupDoneCallback();
        }

        public override void Exit()
        {
            Debug.LogFormat("Setup state {0} unloaded.", "GameplayState");
        }
    }
}
