﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class SetUpTracker : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {
            myContext = (AppFlowContext)context;
            base.Enter();
            TrackerManager tm = GameObject.FindObjectOfType<TrackerManager>();
            tm.SetUpTrackers();

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
