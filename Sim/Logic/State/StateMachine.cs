using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Entities;
using Sim.Objects;

namespace Sim.Logic.State
{
    public class StateMachine : ITickable
    {
        private HashSet<IState> states = new HashSet<IState>();
        private IState currentState;

        private IEntity entity;
        private IObject obj;

        public IEntity Entity => entity;
        public IObject Object => obj;

        public StateMachine(IEntity entity)
        {
            this.entity = entity;
        }

        public StateMachine(IObject obj)
        {
            this.obj = obj;
        }

        public StateMachine AddState<T>(T state) where T : IState
        {
            if(states.OfType<T>().Any())
            {
                return this;
            }

            state.Entity = entity;
            state.Object = obj;
            states.Add(state);

            return this;
        }

        public T GetState<T>() where T : IState
        {
            return states.OfType<T>().FirstOrDefault();
        }

        public T GetCurrentState<T>() where T : IState
        {
            return (T)currentState;
        }

        public StateMachine RemoveState<T>() where T : IState
        {
            if (!states.OfType<T>().Any())
            {
                return this;
            }

            states.Remove(states.OfType<T>().First());

            return this;
        }

        public StateMachine SwitchState<T>() where T : IState
        {
            var state = states.OfType<T>().First();
            currentState = state;
            return this;
        }

        public void Tick()
        {
            currentState?.Tick();
        }
    }
}
