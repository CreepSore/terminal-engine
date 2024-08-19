using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Sim.Entities;
using Sim.Logic;
using Sim.Objects;
using Sim.Structs;

namespace Sim.World.Populator
{
    public class MainWorldPopulator : IWorldPopulator
    {
        private World world;
        private Random random;

        public MainWorldPopulator(World world, int? seed = null)
        {
            this.world = world;
            if (seed != null)
            {
                random = new Random((int)seed);
            }
            else
            {
                random = new Random();
            }
        }

        public void Populate()
        {
            for (int x = 0; x < world.Width; x += 10 * random.Next(50, 150) / 100)
            {
                for (int y = 0; y < world.Height; y += 10 * random.Next(50, 150) / 100)
                {
                    if (random.Next(0, 100) > 95)
                    {
                        GenerateBlob(
                            new Vec3d(x, y, 0),
                            50,
                            300,
                            1000,
                            2000,
                            () => new ObjectTree(Vec3d.Zero)
                        );
                    }
                }
            }
        }

        private void GenerateBlob(
            Vec3d position,
            int minStrengthStepdown,
            int maxStrengthStepdown,
            int minStartStrength,
            int maxStartStrength,
            Func<IWithPosition> generator
        )
        {
            IList<Vec3d> nodes = new List<Vec3d>() { position };
            IList<Vec3d> queue = new List<Vec3d>() { position };
            IList<Vec3d> generatedPositions = new List<Vec3d>() { };
            IDictionary<Vec3d, int> strengths = new Dictionary<Vec3d, int>() { { position, random.Next(minStartStrength, maxStartStrength) }};

            while (queue.Count > 0)
            {
                var current = queue[random.Next(0, queue.Count - 1)];
                var currentStrength = strengths[current];
                queue.Remove(current);

                if (generatedPositions.All(vec => vec != current))
                {
                    var generated = generator();
                    if (generated is IObject obj)
                    {
                        world.SpawnObject(obj, current);
                    }
                    else if (generated is IEntity entity)
                    {
                        world.SpawnEntity(entity, current);
                    }

                    generatedPositions.Add(current);
                }

                foreach (var neighbor in current.GetNeighbors())
                {
                    if (neighbor.X < 0 || neighbor.Y < 0 || neighbor.X >= world.Width || neighbor.Y >= world.Height)
                    {
                        continue;
                    }

                    if (nodes.All(n => n != neighbor))
                    {
                        nodes.Add(neighbor);
                    }

                    var node = nodes.First(vec => vec == neighbor);

                    if (generatedPositions.Any(vec => vec == node))
                    {
                        continue;
                    }

                    var nextStrength = currentStrength - random.Next(minStrengthStepdown, maxStrengthStepdown);

                    if (nextStrength <= 0)
                    {
                        continue;
                    }

                    strengths[node] = nextStrength;
                    queue.Add(node);
                }
            }
        }
    }
}
