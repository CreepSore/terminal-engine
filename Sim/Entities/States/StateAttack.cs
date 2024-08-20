using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Capabilities;
using Sim.Const;
using Sim.Logic;
using Sim.Logic.State;
using Sim.Objects;

namespace Sim.Entities.States
{
    public class StateAttack : IState
    {
        public IWithPosition PositionObject => Entity != null ? (IWithPosition)Entity : Object;
        public IWithCapabilities CapabilityObject => Entity != null ? (IWithCapabilities)Entity : Object;
        public IEntity Entity { get; set; }
        public IObject Object { get; set; }

        public ICapabilityObject target;

        public void Tick()
        {
            if (target == null)
            {
                return;
            }

            var attack = CapabilityObject.GetCapability<CapabilityAttack>();

            if (attack == null)
            {
                return;
            }

            attack.Attack(target, 100, DamageType.Punch);
        }

        public void SetTarget(IObject target)
        {
            this.target = target;
        }
    }
}
