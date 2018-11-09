using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Physiotherapy.StateMachine
{
    public interface IState
    {
        IState Setup(IContext ctx);

        void Enter();
        void Tick();
        void Exit();
    }

    public abstract class BaseState : IState
    {

        protected IContext context;

        public IState Setup(IContext ctx)
        {
            context = ctx;
            Debug.LogFormat("Setup state {0} done.", this.GetType());
            return this;
        }

        public virtual void Enter()
        {

        }

        public virtual void Exit()
        {

        }

        public virtual void Tick()
        {

        }

    }

    /// <summary>
    /// Interfaccia comune per i contesti della state machine.
    /// </summary>
    public interface IContext
    {

    }
}
