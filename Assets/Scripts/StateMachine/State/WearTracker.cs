using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class WearTracker : BaseState
    {

        AppFlowContext myContext;

        TrackerManager tm;
        public override void Enter()
        {

            myContext = (AppFlowContext)context;
            base.Enter();
            tm = GameObject.FindObjectOfType<TrackerManager>();
            tm.SetUpTrackers();

        }


        public override void Tick()
        {

            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
            if (tm.setUpTrackerDone)
                myContext.TrackerWearDoneCallback();
        }

        public override void Exit()
        {

        }
    }
}
