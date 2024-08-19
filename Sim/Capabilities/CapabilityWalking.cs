using Sim.Const;
using Sim.Pathfinding;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Capabilities
{
    public class CapabilityWalking : BaseCapability
    {
        public double WalkingSpeed { get; set; }

        private Path currentPath;
        private Vec3d? currentTargetPosition;
        private int idleTicks = 0;

        public bool NoLimits { get; set; } = false;

        public CapabilityWalking(double walkingSpeed = 1.0f)
        {
            WalkingSpeed = walkingSpeed;
        }

        public override void Tick()
        {
            if(idleTicks > 0)
            {
                idleTicks -= (int)(1 * WalkingSpeed);

                if (idleTicks < 0)
                {
                    idleTicks = 0;
                }
            }
        }

        public bool Walk(Direction direction)
        {
            if(!CanWalkNow())
            {
                return false;
            }

            var newPosition = PositionObject.Position + Vec3d.FromDirection(direction);

            if(!PositionObject.World.IsWalkable(newPosition, GetCollisionLayers()))
            {
                return false;
            }

            PositionObject.Position = newPosition;
            
            if(Entity != null)
            {
                idleTicks = 20;
            }

            return true;
        }

        public bool Walk(Vec3d targetPosition)
        {
            if(!CanWalkNow())
            {
                return false;
            }

            if(targetPosition == currentTargetPosition && currentPath != null)
            {
                if(currentPath.Positions.Count == 0 && PositionObject.Position == targetPosition)
                {
                    if (targetPosition == PositionObject.Position)
                    {
                        return true;
                    }

                    return false;
                }
                else if(currentPath.Positions.Count != 0)
                {
                    var nextPosition = currentPath.Positions.First();
                    

                    var direction = PositionObject.Position.GetDirectionTo(nextPosition);
                    if (Walk(direction))
                    {
                        currentPath.Positions.RemoveAt(0);
                    }

                    return false;
                }
            }

            currentTargetPosition = targetPosition;
            var costMatrix = CostMatrix.FromWorld(PositionObject.World, GetCollisionLayers());
            costMatrix.SetValue(targetPosition, 0);
            currentPath = new AStarPathFinder(costMatrix).FindPath(PositionObject.Position, targetPosition);

            Walk(targetPosition);

            return true;
        }

        public bool CanWalkNow()
        {
            return NoLimits || idleTicks <= 0;
        }

        private IList<CollisionLayers> GetCollisionLayers()
        {
            IList<CollisionLayers> collisionLayers = null;

            if (Entity != null)
            {
                collisionLayers = Entity.GetCapability<CapabilityCollision>()?.Layers;
            }
            else if (Object != null)
            {
                collisionLayers = Object.GetCapability<CapabilityCollision>()?.Layers;
            }

            return collisionLayers ?? new List<CollisionLayers>();
        }
    }
}
