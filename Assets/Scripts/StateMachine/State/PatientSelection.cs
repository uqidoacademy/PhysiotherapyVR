using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UI.Desktop;

namespace Physiotherapy.StateMachine
{
    public class PatientSelection : BaseState
    {

        AppFlowContext myContext;

        public override void Enter()
        {
           
            myContext = (AppFlowContext)context;

            myContext.ListPatient = new List<PatientProfile>();
            myContext.ListPatient.Add(new PatientProfile("MR", "Mario",
                "Rossi", "Male", 1.7f, true, "Broken shoulder"));
            UIDesktopManager.I.ActiveSelectionPatientPanel(myContext.ListPatient);

                
            base.Enter();
        }

        
        public override void Tick()
        {
            if (Input.GetKeyDown(KeyCode.A))
                myContext.DoneCallBack();
        }

        public void DoneSelectionPatient()
        {
            //TODO: mettere il paziente selezionato, non il primo della lista
            myContext.currentPatient = myContext.ListPatient[0];

            myContext.DoneCallBack();
        }

        public override void Exit()
        {
           
        }
    }
}
