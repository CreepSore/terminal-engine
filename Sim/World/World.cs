using Sim.Capabilities;
using Sim.Const;
using Sim.Entities;
using Sim.Logic;
using Sim.Objects;
using Sim.Rendering;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.World
{
    public class World : ITickable, IRenderable
    {
        public IList<IRenderable> RenderableChildren => renderables;
        public Vec3d Position { get; set; } = Vec3d.Zero;
        private readonly IList<IRenderable> renderables = new List<IRenderable>();
        private readonly IList<IEntity> entities = new List<IEntity>();
        private readonly IList<IObject> objects = new List<IObject>();
        private readonly IList<IWithPosition> positionObjects = new List<IWithPosition>();
        private readonly IList<ICapabilityObject> capabilityObjects = new List<ICapabilityObject>();

        public int Width { get; }
        public int Height { get; }

        public Dictionary<Vec3d, IList<ICapabilityObject>> capabilityObjectMatrix = new Dictionary<Vec3d, IList<ICapabilityObject>>();

        World IWithPosition.World { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public World(int width, int height) {
            Width = width;
            Height = height;
        }

        #region Implements ITickable
        public void Tick()
        {
            foreach(var entity in entities)
            {
                entity.Tick();
            }
        }
        #endregion

        #region Spawn Functions
        public World SpawnEntity(IEntity entity, Vec3d? position = null, bool nonColliding = true)
        {
            if (entities.Contains(entity))
            {
                return this;
            }

            var checkPos = (position ?? entity.Position);
            if (
                checkPos.X < 0 || checkPos.Y < 0
                || checkPos.X >= Width || checkPos.Y >= Height
            )
            {
                return this;
            }

            if (position != null)
            {
                entity.Position = (Vec3d)position;
            }

            if (nonColliding)
            {
                while (!IsWalkable(
                   entity.Position,
                   entity.GetCapability<CapabilityCollision>()?.Layers ?? new List<CollisionLayers>()
                ))
                {
                    entity.Position += Vec3d.Right;
                }
            }

            entity.World = this;

            entities.Add(entity);
            positionObjects.Add(entity);
            capabilityObjects.Add(entity);
            renderables.Add(entity);
            if (!capabilityObjectMatrix.ContainsKey(entity.Position))
            {
                capabilityObjectMatrix[entity.Position] = new List<ICapabilityObject>();
            }
            capabilityObjectMatrix[entity.Position].Add(entity);

            return this;
        }

        public World SpawnObject(IObject obj, Vec3d? position = null, bool nonColliding = true)
        {
            if (objects.Contains(obj))
            {
                return this;
            }

            var realPos = (position ?? obj.Position);
            if (
                realPos.X < 0 || realPos.Y < 0
                || realPos.X >= Width || realPos.Y >= Height
            )
            {
                return this;
            }

            if (position != null)
            {
                obj.Position = (Vec3d)position;
            }

            if (nonColliding)
            {
                while (!IsWalkable(
                    obj.Position,
                    obj.GetCapability<CapabilityCollision>()?.Layers ?? new List<CollisionLayers>()
                ))
                {
                    obj.Position += Vec3d.Right;
                }
            }

            obj.World = this;

            objects.Add(obj);
            positionObjects.Add(obj);
            capabilityObjects.Add(obj);
            renderables.Add(obj);
            if (!capabilityObjectMatrix.ContainsKey(obj.Position))
            {
                capabilityObjectMatrix[obj.Position] = new List<ICapabilityObject>();
            }
            capabilityObjectMatrix[obj.Position].Add(obj);

            return this;
        }

        public World DespawnEntity(IEntity entity)
        {
            if (!entities.Contains(entity))
            {
                return this;
            }

            entity.World = null;

            entities.Remove(entity);
            positionObjects.Remove(entity);
            capabilityObjects.Remove(entity);
            renderables.Remove(entity);
            capabilityObjectMatrix[entity.Position].Remove(entity);

            return this;
        }

        public World DespawnObject(IObject obj)
        {
            if (!objects.Contains(obj))
            {
                return this;
            }

            obj.World = null;

            objects.Remove(obj);
            positionObjects.Remove(obj);
            capabilityObjects.Remove(obj);
            renderables.Remove(obj);
            capabilityObjectMatrix[obj.Position].Remove(obj);

            return this;
        }
        #endregion

        #region Entity Functions
        public IList<IEntity> GetEntities()
        {
            return entities;
        }

        public IEnumerable<IEntity> GetEntities(Vec3d position)
        {
            return entities.Where(e => position == e.Position).ToList();
        }

        public bool HasEntitiesAt(Vec3d position)
        {
            return entities.Any(e => position == e.Position);
        }

        public bool HasEntitiesAt(Vec3d position, Func<IEntity, bool> filter)
        {
            return entities.Any(e => position == e.Position && filter(e));
        }
        #endregion

        #region Object Functions
        public IList<IObject> GetObjects()
        {
            return objects;
        }

        public bool HasObjectsAt(Vec3d position)
        {
            return objects.Any(o => position == o.Position);
        }

        public bool HasObjectsAt(Vec3d position, Func<IObject, bool> filter)
        {
            return objects.Any(o => position == o.Position && filter(o));
        }

        public IEnumerable<IObject> GetObjects(Vec3d position)
        {
            return objects.Where(e => position == e.Position);
        }
        #endregion

        #region PositionObject Functions
        public IList<IWithPosition> GetPositionObjects()
        {
            return positionObjects;
        }

        public bool HasPositionObjectAt(Vec3d position)
        {
            return positionObjects.Any(e => position == e.Position);
        }
        
        public bool HasPositionObjectAt(Vec3d position, Func<IWithPosition, bool> filter)
        {
            return positionObjects.Any(o => position == o.Position && filter(o));
        }

        public IEnumerable<IWithPosition> GetPositionObjects(Vec3d position)
        {
            return positionObjects.Where(e => position == e.Position);
        }
        #endregion

        #region CapabilityObjects Functions
        public IList<ICapabilityObject> GetCapabilityObjects()
        {
            return capabilityObjects;
        }

        public IEnumerable<ICapabilityObject> GetCapabilityObjects<CapabilityType>() where CapabilityType : ICapability
        {
            return capabilityObjects
                .Where(wc => wc.HasCapability<CapabilityType>());
        }

        public IList<ICapabilityObject> GetCapabilityObjects(Vec3d position)
        {
            if (!capabilityObjectMatrix.TryGetValue(position, out var objects))
            {
                return new List<ICapabilityObject>();
            }

            return objects;
        }

        public IEnumerable<ICapabilityObject> GetCapabilityObjects<CapabilityType>(Vec3d position) where CapabilityType : ICapability
        {
            return GetCapabilityObjects(position).Where(wc => wc.HasCapability<CapabilityType>());
        }
        #endregion

        public void HandlePositionChanged(ICapabilityObject capabilityObject, Vec3d newPosition)
        {
            capabilityObjectMatrix[capabilityObject.Position].Remove(capabilityObject);

            if (!capabilityObjectMatrix.ContainsKey(newPosition))
            {
                capabilityObjectMatrix[newPosition] = new List<ICapabilityObject>();
            }
            capabilityObjectMatrix[newPosition].Add(capabilityObject);
        }

        public bool IsWalkable(Vec3d position, IList<CollisionLayers> layers = null)
        {
            var toCheck = layers ?? new List<CollisionLayers>() { CollisionLayers.Default };

            if (toCheck.Count == 0)
            {
                return true;
            }

            return !GetCapabilityObjects<CapabilityCollision>(position)
                .Select(co => co.GetCapability<CapabilityCollision>())
                .Any(c => c.CanCollideWith(toCheck));
        }
    }
}
