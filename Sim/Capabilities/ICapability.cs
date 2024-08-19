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
    public interface ICapability : ITickable
    {
        IWithPosition PositionObject { get; }
        IEntity Entity { get; set; }
        IObject Object { get; set; }

        /// <summary>
        /// Will only be called if the capability parent is an <see cref="IEntity"/>
        /// </summary>
        new void Tick();

        void OnAttached();
    }
}
