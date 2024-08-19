using Sim.Capabilities;
using Sim.Const;
using Sim.Logic;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Pathfinding
{
    public class CostMatrix
    {
        private const int UnwalkableValue = int.MinValue;

        private int width;
        private int height;

        private int[,] matrix;

        public CostMatrix(int width, int height)
        {
            this.width = width;
            this.height = height;
            matrix = new int[width, height];
        }

        public int GetValue(Vec3d position)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height)
            {
                return UnwalkableValue;
            }

            return matrix[(int)position.X, (int)position.Y];
        }

        public void SetValue(Vec3d position, int value)
        {
            if (position.X < 0 || position.Y < 0 || position.X >= width || position.Y >= height)
            {
                return;
            }

            matrix[(int)position.X, (int)position.Y] = value;
        }

        public bool IsWalkable(Vec3d position)
        {
            return position.X >= 0 && position.Y >= 0
                && position.X < width && position.Y < height
                && GetValue(position) != UnwalkableValue;
        }

        public void SetUnwalkable(Vec3d position)
        {
            SetValue(position, UnwalkableValue);
        }

        public static CostMatrix FromWorld(World.World world, IList<CollisionLayers> collisionLayers = null)
        {
            var colLayers = collisionLayers ?? new List<CollisionLayers>() { CollisionLayers.Default };
            var costMatrix = new CostMatrix(world.Width, world.Height);

            foreach(var positionObject in world.GetCapabilityObjects<CapabilityCollision>())
            {
                if (!world.IsWalkable(positionObject.Position, colLayers))
                {
                    continue;
                }
                costMatrix.SetUnwalkable(positionObject.Position);
            }

            return costMatrix;
        }
    }
}
