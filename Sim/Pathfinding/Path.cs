using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Pathfinding
{
    public class Path
    {
        public IList<Vec3d> Positions { get; set; } = new List<Vec3d>();
    }
}
