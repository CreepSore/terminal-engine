using Sim.Const;
using Sim.Entities;
using Sim.Logic;
using Sim.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sim.Capabilities
{
    public class CapabilityCollision : BaseCapability
    {
        public IList<CollisionLayers> Layers { get; private set; }
        public IEntity Entity { get; set; }
        public IObject Object { get; set; }
        public IWithPosition PositionObject {
            get {
                if(Entity != null)
                {
                    return Entity;
                }

                if(Object != null)
                {
                    return Object;
                }

                return null;
            }
        }

        public CapabilityCollision(IList<CollisionLayers> layers)
        {
            Layers = layers;
        }

        public override void Tick()
        {
            
        }

        public bool CanCollideWith(CapabilityCollision collision)
        {
            return CanCollideWith(collision.Layers);
        }

        public bool CanCollideWith(IList<CollisionLayers> layers)
        {
            return layers.Any(l => Layers.Contains(l));
        }
    }
}
