using Sim.Capabilities;
using Sim.Rendering;
using Sim.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Objects
{
    public abstract class BaseObject : IObject
    {
        public IList<IRenderable> RenderableChildren => renderableChildren;

        private Vec3d position = Vec3d.Zero;
        public Vec3d Position
        {
            get => position;
            set
            {
                if (World != null)
                {
                    World.HandlePositionChanged(this, value);
                }

                position = value;
            }
        }

        public IList<ICapability> Capabilities => new List<ICapability>();

        public World.World World { get; set; }

        private IList<IRenderable> renderableChildren = new List<IRenderable>();
        private IList<ICapability> capabilities = new List<ICapability>();

        public void AddCapability(ICapability capability)
        {
            if (capabilities.Any(c => c.GetType() == capability.GetType()))
            {
                return;
            }

            capability.Object = this;
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
    }
}
