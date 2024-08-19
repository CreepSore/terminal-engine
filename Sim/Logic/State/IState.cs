using Sim.Entities;
using Sim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Logic.State
{
    public interface IState : ITickable
    {
        IWithPosition PositionObject { get; }
        IEntity Entity { get; set; }
        IObject Object { get; set; }
    }
}
