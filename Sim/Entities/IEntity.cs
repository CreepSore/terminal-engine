using Sim.Capabilities;
using Sim.Logic;
using Sim.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Entities
{
    public interface IEntity : ITickable, IRenderable, ICapabilityObject
    {
    }
}
