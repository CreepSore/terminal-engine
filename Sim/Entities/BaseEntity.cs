using Sim.Capabilities;
using Sim.Rendering;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Entities
{
    public abstract class BaseEntity : IEntity
    {
        public IList<IRenderable> RenderableChildren => renderableChildren;
        private Vec3d position = Vec3d.Zero;
        public Vec3d Position
        {
            get => position;
            set
            {
                World?.HandlePositionChanged(this, value);

                position = value;
            }
        }

        public IList<ICapability> Capabilities => new List<ICapability>();

        public World.World World { get; set; }

        private IList<IRenderable> renderableChildren = new List<IRenderable>();
        private IList<ICapability> capabilities = new List<ICapability>();

        public void UpdatePosition(Action updateFunction)
        {
            var oldPosition = new Vec3d(position);
            updateFunction();
            World?.HandlePositionChanged(this, position);
        }

        public void AddCapability(ICapability capability)
        {
            if(capabilities.Any(c => c.GetType() == capability.GetType()))
            {
                return;
            }

            capability.Entity = this;
            capabilities.Add(capability);
        }

        public T GetCapability<T>() where T : ICapability
        {
            return capabilities.OfType<T>().FirstOrDefault();
        }

        public bool HasCapability<T>() where T : ICapability
        {
            return GetCapability<T>() != null;
        }

        public void TickCapabilities()
        {
            foreach(var capability in capabilities)
            {
                capability.Tick();
            }
        }

        abstract public void Tick();
    }
}
