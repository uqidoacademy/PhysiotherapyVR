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

            // iscrivo 3 funzioni agli eventi che saranno chiamati in base al fatto che l'utente voglia finire, voglia rifare es con stessa parte del corpo, voglia cambiare parte corpo
            UIDesktopManager.EventEndExperience += GoToNextState;
            UIDesktopManager.EventReDoExerciseSameBodyPart += ReDoExerciseSameBodyPart;
            UIDesktopManager.EventReDoExerciseDifferentBodyPart += ReDoExerciseDifferentBodyPart;

            myContext = (AppFlowContext)context;
            
            UIDesktopManager.I.ActiveTrackersFeedbackPanel(myContext.currentBodyPart);

            base.Enter();

            // Quick Fix!!!
            foreach (var gizmo in GameObject.FindObjectsOfType<TolleranceGizmo>())
            {
                // niente... eliminato perchè la tolleranza che viene passata quì non è quella corretta...
                //gizmo.radius = myContext.listBodyParts[0].PositionTollerance;
                //gizmo.DrawTolleranceGizmo();
            }
        }


        public override void Tick()
        {

            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
        }

        public override void Exit()
        {
            UIDesktopManager.I.TrackersFeedbackPanel.SetActive(false);
            UIDesktopManager.EventEndExperience -= GoToNextState;
            UIDesktopManager.EventReDoExerciseSameBodyPart -= ReDoExerciseSameBodyPart;
            UIDesktopManager.EventReDoExerciseDifferentBodyPart -= ReDoExerciseDifferentBodyPart;
        }


        #region methodsForEvents
        public void GoToNextState()
        {
            myContext.DoneCallBack();
        }

        public void ReDoExerciseSameBodyPart()
        {
            myContext.FinishExerciseSameBdPartCallback();
        }

        public void ReDoExerciseDifferentBodyPart()
        {
            myContext.FinishExerciseDifferentBdPartCallback();
        }
        #endregion
    }
}
