using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Logic
{
    public interface IWithPosition
    {
        World.World World { get; set; }
        Vec3d Position { get; set; }
    }
}
