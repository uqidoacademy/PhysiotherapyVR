using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRPhysiotheraphyst;

namespace Physiotherapy.StateMachine
{
    public class DoingExercise : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {

            myContext = (AppFlowContext)context;
            
            UIDesktopManager.I.ActiveTrackersFeedbackPanel(myContext.currentBodyPart);

            base.Enter();
        }


        public override void Tick()
        {

            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
        }

        public override void Exit()
        {

        }


    }
}
