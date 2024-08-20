using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Logic;
using Sim.Logic.State;
using Sim.Objects;

namespace Sim.Entities.States
{
    public class StateIdle : IState
    {
        public IWithPosition PositionObject { get; }
        public IEntity Entity { get; set; }
        public IObject Object { get; set; }
        
        public void Tick()
        {
            
        }
    }
}
