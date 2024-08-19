using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Objects;
using Sim.Structs;

namespace Sim.World.Populator
{
    public class FillWorldPopulator : IWorldPopulator
    {
        private readonly World world;

        public FillWorldPopulator(World world)
        {
            this.world = world;
        }

        public void Populate()
        {
            for (int y = 1; y < world.Height; y++)
            {
                for (int x = 0; x < world.Width; x++)
                {
                    world.SpawnObject(new ObjectTree(new Vec3d(x, y, 0)));
                }
            }
        }
    }
}
