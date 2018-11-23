using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public class EndExercise : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {
            Application.Quit();
            myContext = (AppFlowContext)context;

            Debug.Log("HO FINITO L'ESERCIZIO");


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
