using Sim.Capabilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sim.Const;
using Sim.Structs;

namespace Sim.Objects
{
    public class ObjectTree : BaseObject
    {
        public ObjectTree(Vec3d position)
        {
            Position = position;

            var living = new CapabilityLiving(100);
            living.AfterDamage += AfterDamage;
            AddCapability(living);
            
            AddCapability(new CapabilityCollision(new List<CollisionLayers>() { CollisionLayers.Default }));
        }

        private void AfterDamage(object sender, DamageEventArgs e)
        {
            if (GetCapability<CapabilityLiving>().IsDead)
            {
                World.DespawnObject(this);
            }
        }
    }
}
