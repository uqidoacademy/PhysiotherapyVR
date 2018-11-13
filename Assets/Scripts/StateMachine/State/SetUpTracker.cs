using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class SetUpTracker : BaseState
    {

        AppFlowContext myContext;
        TrackerRenamer trackerRenamer;
        public override void Enter()
        {
            myContext = (AppFlowContext)context;
            base.Enter();

            trackerRenamer = GameObject.FindObjectOfType<TrackerRenamer>();
            UIDesktopManager.I.ActiveSetupTrackersPanel(myContext.currentBodyPart);
            trackerRenamer.SetInteraction(true);

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
