using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Capabilities;
using Sim.Const;

namespace Sim.Objects
{
    public class ObjectChest : BaseObject
    {
        public ObjectChest()
        {
            AddCapability(new CapabilityCollision(new List<CollisionLayers>() { CollisionLayers.Default }));
            AddCapability(new CapabilityInventory(32));
        }
    }
}
