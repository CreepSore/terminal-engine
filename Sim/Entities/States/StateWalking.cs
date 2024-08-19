using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Capabilities;
using Sim.Logic;
using Sim.Logic.State;
using Sim.Objects;
using Sim.Structs;

namespace Sim.Entities.States
{
    public class StateWalking : IState
    {
        public IWithPosition PositionObject => Entity != null ? (IWithPosition)Entity : Object;
        public IEntity Entity { get; set; }
        public IObject Object { get; set; }

        private Vec3d? targetPosition;
        private double maximumDistanceToTarget;

        private readonly Action onDestinationReached;
        private bool destinationReachedCalled = false;

        public StateWalking(Action onDestinationReached)
        {
            this.onDestinationReached = onDestinationReached;
        }

        public void Tick()
        {
            if (targetPosition == null)
            {
                return;
            }

            var walking = Entity.GetCapability<CapabilityWalking>();
            if (walking == null) return;

            if (PositionObject.Position.Distance2d((Vec3d)targetPosition) <= maximumDistanceToTarget)
            {
                FireOnDestinationReached();
                return;
            }

            walking.Walk((Vec3d)targetPosition);

            if (PositionObject.Position.Distance2d((Vec3d)targetPosition) <= maximumDistanceToTarget)
            {
                FireOnDestinationReached();
                targetPosition = null;
            }
        }

        public StateWalking SetTargetPosition(Vec3d? position)
        {
            targetPosition = position;
            destinationReachedCalled = false;
            return this;
        }

        public StateWalking SetMaximumDistanceToTarget(double distance)
        {
            maximumDistanceToTarget = distance;
            return this;
        }

        private void FireOnDestinationReached()
        {
            if (destinationReachedCalled)
            {
                return;
            }

            onDestinationReached();
            destinationReachedCalled = true;
        }
    }
}
