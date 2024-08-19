using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Pathfinding
{
    public interface IPathFinder
    {
        Path FindPath(Vec3d from, Vec3d to, Func<Vec3d, Vec3d, bool> matcher = null);
    }
}
