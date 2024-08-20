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
        private readonly HashSet<IState> states = new HashSet<IState>();
        private IState currentState;

        public IEntity Entity { get; }
        public IObject Object { get; }

        public StateMachine(IEntity entity)
        {
            this.Entity = entity;
        }

        public StateMachine(IObject obj)
        {
            this.Object = obj;
        }

        public StateMachine AddState<T>(T state) where T : IState
        {
            if(states.OfType<T>().Any())
            {
                return this;
            }

            state.Entity = Entity;
            state.Object = Object;
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
