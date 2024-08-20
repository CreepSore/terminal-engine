using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Items;
using Sim.Objects;
using Sim.Structs;

namespace Sim.Capabilities
{
    public class CapabilityBuild : BaseCapability
    {
        public override void Tick()
        {
            
        }

        public IObject PlaceItem<T>(T item, Vec3d position, bool forcePosition = false) where T : BuildableItem
        {
            var inventory = CapabilityObject.GetCapability<CapabilityInventory>();
            if (inventory == null)
            {
                return null;
            }

            if (!inventory.HasItem(item))
            {
                return null;
            }

            var itemStack = inventory.GetItemStack(item);
            if (itemStack == null)
            {
                return null;
            }

            return PlaceItem(itemStack, position, forcePosition);
        }

        public IObject PlaceItem(ItemStack item, Vec3d position, bool forcePosition = false)
        {
            if (!(item.Item is BuildableItem buildableItem))
            {
                return null;
            }

            return PlaceObject(buildableItem.GetPlaceableObject(), position, forcePosition);
        }

        public IObject PlaceObject(IObject obj, Vec3d position, bool forcePosition = false)
        {
            var bestPosition = GetBestPosition(position);
            if (bestPosition != position && forcePosition)
            {
                return null;
            }

            PositionObject.World.SpawnObject(obj, bestPosition, false);

            return obj;
        }

        public Vec3d? GetBestPosition(Vec3d position)
        {
            if (!PositionObject.World.HasPositionObjectAt(position))
            {
                return position;
            }

            return position
                .GetNeighbors()
                .OfType<Vec3d?>()
                .FirstOrDefault(n => !PositionObject.World.HasPositionObjectAt((Vec3d)n));
        }
    }
}
