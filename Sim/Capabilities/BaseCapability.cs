using Sim.Entities;
using Sim.Logic;
using Sim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Capabilities
{
    public abstract class BaseCapability : ICapability
    {
        public IWithPosition PositionObject => Entity != null ? (IWithPosition)Entity : Object;
        public ICapabilityObject CapabilityObject => Entity != null ? (ICapabilityObject)Entity : Object;

        public IEntity Entity { get; set; }
        public IObject Object { get; set; }

        public virtual void Tick()
        {

        }

        public virtual void OnAttached()
        {
            
        }
    }
}
